import pandas as pd
from ortools.sat.python import cp_model
from datetime import timedelta

ENABLE_ONLINE_RESTRICTION = False
ENABLE_MINIMIZE_GAPS = False

classrooms_df = pd.read_csv('../DataInput/Classrooms.csv')
schedule_groups_days_df = pd.read_csv('../DataInput/ScheduleGroupsDays.csv')
preliminary_schedule_df = pd.read_csv('../DataInput/PreliminaryCoursesSchedule.csv')

schedule_groups_days_df['DateTimeStart'] = pd.to_datetime(schedule_groups_days_df['DateTimeStart'])
schedule_groups_days_df['DateTimeEnd']   = pd.to_datetime(schedule_groups_days_df['DateTimeEnd'])

preliminary_schedule_df['DateTimeStart'] = pd.to_datetime(preliminary_schedule_df['DateTimeStart'])
preliminary_schedule_df['DateTimeEnd']   = pd.to_datetime(preliminary_schedule_df['DateTimeEnd'])

preliminary_schedule_df['DurationMinutes'] = (
    preliminary_schedule_df['DateTimeEnd'] - preliminary_schedule_df['DateTimeStart']
).dt.total_seconds() / 60

preliminary_schedule_df = preliminary_schedule_df.rename(columns={
    'CourseScheduleId': 'course_id',
    'CourseName':       'course_name',
    'InstructorId':     'instructor_id',
    'GroupsName':       'groups_name',
    'ScheduleGroupId':  'schedule_group_id',
    'DateTimeStart':    'datetime_start',
    'DateTimeEnd':      'datetime_end',
    'Duration':         'duration',
    'ClassroomId':      'classroom_id',
    'CourseType':       'course_type'
})

model = cp_model.CpModel()
course_vars   = {}
interval_vars = {}

origin = pd.Timestamp('2024-10-01')

for _, row in preliminary_schedule_df.iterrows():
    cid    = row['course_id']
    grp    = row['groups_name']
    dur_bl = int(row['duration'] // 10)

    slots = schedule_groups_days_df[
        schedule_groups_days_df['ScheduleGroupId'] == row['schedule_group_id']
    ]
    if slots.empty:
        raise ValueError(f"Brak slotów dla grupy {grp}")

    starts, ends, intervals, presences = [], [], [], []

    for slot_idx, slot in slots.iterrows():
        b0 = int((slot['DateTimeStart'] - origin).total_seconds() // 60 // 10)
        b1 = int((slot['DateTimeEnd']   - origin).total_seconds() // 60 // 10)

        b = b0
        while b + dur_bl <= b1:
            p = model.NewBoolVar(f"pres_{cid}_{slot_idx}_{b}")
            s = model.NewIntVar(b, b, f"st_{cid}_{slot_idx}_{b}")
            e = model.NewIntVar(b + dur_bl, b + dur_bl, f"en_{cid}_{slot_idx}_{b}")
            iv = model.NewOptionalIntervalVar(s, dur_bl, e, p, f"iv_{cid}_{slot_idx}_{b}")

            starts.append(s)
            ends.append(e)
            intervals.append(iv)
            presences.append(p)

            b += dur_bl

    model.AddExactlyOne(presences)
    course_vars[cid]   = (starts, ends, presences)
    interval_vars[cid] = intervals

for grp in preliminary_schedule_df['groups_name'].unique():
    courses = preliminary_schedule_df[
        preliminary_schedule_df['groups_name'] == grp
    ]['course_id']
    all_iv = sum((interval_vars[c] for c in courses), [])
    model.AddNoOverlap(all_iv)

if 'instructor_id' in preliminary_schedule_df.columns:
    for instr in preliminary_schedule_df['instructor_id'].dropna().unique():
        courses = preliminary_schedule_df[
            preliminary_schedule_df['instructor_id'] == instr
        ]['course_id']
        all_iv = sum((interval_vars[c] for c in courses), [])
        model.AddNoOverlap(all_iv)

if ENABLE_ONLINE_RESTRICTION and 'Online' in preliminary_schedule_df.columns:
    for grp in preliminary_schedule_df['groups_name'].unique():
        dfg = preliminary_schedule_df[preliminary_schedule_df['groups_name'] == grp]
        onl = dfg[dfg['Online'] == 'Yes']
        stn = dfg[dfg['Online'] == 'No']
        for _, r1 in onl.iterrows():
            for _, r2 in stn.iterrows():
                c1, c2 = r1['course_id'], r2['course_id']
                s1, _, p1 = course_vars[c1]
                s2, _, p2 = course_vars[c2]
                for i, lit1 in enumerate(p1):
                    for j, lit2 in enumerate(p2):
                        d1 = model.NewIntVar(0, 1000, f"d1_{c1}_{i}")
                        d2 = model.NewIntVar(0, 1000, f"d2_{c2}_{j}")
                        model.AddDivisionEquality(d1, s1[i], 6*24).OnlyEnforceIf([lit1, lit2])
                        model.AddDivisionEquality(d2, s2[j], 6*24).OnlyEnforceIf([lit1, lit2])
                        model.Add(d1 != d2).OnlyEnforceIf([lit1, lit2])

solver = cp_model.CpSolver()
solver.parameters.max_time_in_seconds = 3600.0
status = solver.Solve(model)

if status in (cp_model.OPTIMAL, cp_model.FEASIBLE):
    print("Znaleziono rozwiązanie!\n")
    rows = []
    for _, row in preliminary_schedule_df.iterrows():
        cid   = row['course_id']
        name  = row['course_name']
        ctype = row['course_type']    #
        grp   = row['groups_name']
        dur   = int(row['duration'])
        starts, _, pres = course_vars[cid]

        for i, lit in enumerate(pres):
            if solver.BooleanValue(lit):
                b0 = solver.Value(starts[i])
                break
        else:
            continue

        t0 = origin + timedelta(minutes=b0 * 10)
        t1 = t0 + timedelta(minutes=dur)

        print(f"{name} [{ctype}] ({grp}) → start: {t0}, koniec: {t1}")
        rows.append({
            'course_id':     cid,
            'course_type':   ctype,
            'groups_name':   grp,
            'course_name':   name,
            'start_datetime': t0,
            'end_datetime':   t1
        })

    out_df = pd.DataFrame(rows)
    out_df.to_csv('../DataOutput/PreGeneratedSchedule.csv', index=False, encoding='utf-8')
    print("\nZapisano do PreGeneratedSchedule.csv")
else:
    print("Brak rozwiązania. Status:", status)
