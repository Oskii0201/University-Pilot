import pandas as pd

preliminary_df = pd.read_csv('../DataInput/PreliminaryCoursesSchedule.csv')
generated_df = pd.read_csv('../DataOutput/PreGeneratedSchedule.csv')
assigned_classrooms_df = pd.read_csv('../DataOutput/assigned_classrooms.csv')

preliminary_df = preliminary_df.merge(
    generated_df[['course_id', 'start_datetime', 'end_datetime']],
    left_on='CourseScheduleId',
    right_on='course_id',
    how='left'
)

preliminary_df.rename(
    columns={
        'start_datetime': 'NewDateTimeStart',
        'end_datetime': 'NewDateTimeEnd'
    },
    inplace=True
)

preliminary_df = preliminary_df.merge(
    assigned_classrooms_df[['course_id', 'Sala_Przydzielona']],
    left_on='CourseScheduleId',
    right_on='course_id',
    how='left'
)

preliminary_df['ClassroomId'] = preliminary_df.apply(
    lambda row: row['Sala_Przydzielona'] if pd.notna(row['Sala_Przydzielona']) else "0",
    axis=1
)

preliminary_df['Classroom'] = preliminary_df.apply(
    lambda row: row['Sala_Przydzielona'] if pd.notna(row['Sala_Przydzielona']) else "",
    axis=1
)

preliminary_df.drop(columns=['course_id_x', 'course_id_y', 'Sala_Przydzielona'], inplace=True)
preliminary_df.to_csv('../DataOutput/UpdatedPreliminaryCoursesSchedule.csv', index=False)

classrooms_df = pd.read_csv('../DataInput/Classrooms.csv')  # Upewnij się, że ścieżka jest poprawna

preliminary_df = preliminary_df.merge(
    classrooms_df[['ClassroomId', 'Number']],
    left_on='Classroom',
    right_on='Number',
    how='left'
)

preliminary_df['MappedClassroomId'] = preliminary_df['ClassroomId_y']
preliminary_df['ClassroomId'] = preliminary_df['MappedClassroomId'].fillna(0).astype(int)

preliminary_df.drop(columns=['Number', 'ClassroomId_y', 'ClassroomId_x', 'MappedClassroomId'], inplace=True)

preliminary_df.to_csv('../DataOutput/UpdatedPreliminaryCoursesSchedule.csv', index=False)

generowanyplan_df = preliminary_df[['CourseScheduleId', 'ClassroomId', 'NewDateTimeStart', 'NewDateTimeEnd']]
generowanyplan_df.to_csv('../DataOutput/GeneratedSchedule.csv', index=False)

print("Pliki UpdatedPreliminaryCoursesSchedule.csv i GeneratedSchedule.csv zostały zaktualizowane i zapisane!")