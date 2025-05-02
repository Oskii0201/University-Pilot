import pandas as pd
from ortools.sat.python import cp_model

schedule_df = pd.read_csv('../DataOutput/PreGeneratedSchedule.csv')
preferences_df = pd.read_csv('../DataOutput/TopN_Classroom_Predictions.csv')

map_rodzaj = {
    "Lecture": "Wykład",
    "Exercises": "Cwiczenia",
    "Laboratory": "Laboratorium",
    "Seminar": "Seminarium",
    "Project": "Projekt"
}

schedule_df = schedule_df.rename(columns={
    'course_type': 'Rodzaj',
    'course_name': 'Przedmiot'
})
schedule_df['Rodzaj'] = schedule_df['Rodzaj'].map(map_rodzaj).fillna('')

schedule_df = schedule_df[schedule_df['Rodzaj'] != 'Wykład']

preferences_df = preferences_df.drop_duplicates(subset=['Przedmiot', 'Rodzaj'])

df = pd.merge(schedule_df, preferences_df, on=['Przedmiot', 'Rodzaj'], how='left')
# 5. Przygotowanie czasu
df['start_datetime'] = pd.to_datetime(df['start_datetime'])
df['end_datetime'] = pd.to_datetime(df['end_datetime'])
df['Duration'] = (df['end_datetime'] - df['start_datetime']).dt.total_seconds() // 60
df['GodzinaOd'] = df['start_datetime'].dt.hour * 60 + df['start_datetime'].dt.minute
df['Data'] = df['start_datetime'].dt.date

top_n_columns = ['Top1', 'Top2', 'Top3', 'Top4', 'Top5']
all_classrooms = sorted({room for col in top_n_columns for room in df[col].dropna().unique()})

classroom_to_id = {room: i for i, room in enumerate(all_classrooms)}
id_to_classroom = {i: room for room, i in classroom_to_id.items()}


model = cp_model.CpModel()
n_lessons = len(df)
room_vars = [model.NewIntVar(0, len(all_classrooms) - 1, f'room_{i}') for i in range(n_lessons)]

penalty_terms = []
for idx, row in df.iterrows():
    penalties = [5] * len(all_classrooms)
    for col in top_n_columns:
        room = row[col]
        if pd.notna(room) and room in classroom_to_id:
            penalties[classroom_to_id[room]] = 0

    penalty = model.NewIntVar(0, 5, f'penalty_{idx}')
    model.AddElement(room_vars[idx], penalties, penalty)
    penalty_terms.append(penalty)

model.Minimize(sum(penalty_terms))

grouped = df.groupby(['Data'])

for date, group in grouped:
    lessons = group.reset_index()
    n = len(lessons)

    for i in range(n):
        start_i = lessons.loc[i, 'GodzinaOd']
        end_i = start_i + lessons.loc[i, 'Duration']

        for j in range(i + 1, n):
            start_j = lessons.loc[j, 'GodzinaOd']
            end_j = start_j + lessons.loc[j, 'Duration']

            if not (end_i <= start_j or end_j <= start_i):
                idx_i = lessons.loc[i, 'index']
                idx_j = lessons.loc[j, 'index']
                model.Add(room_vars[idx_i] != room_vars[idx_j])

solver = cp_model.CpSolver()
solver.parameters.max_time_in_seconds = 300.0
status = solver.Solve(model)

if status in [cp_model.OPTIMAL, cp_model.FEASIBLE]:
    assigned_classrooms = [id_to_classroom[solver.Value(var)] for var in room_vars]
    df['Sala_Przydzielona'] = assigned_classrooms
    df.to_csv('../DataOutput/assigned_classrooms.csv', index=False)
    print("Zapisano wynik do 'assigned_classrooms.csv'")
else:
    print("Nie udało się znaleźć rozwiązania.")
