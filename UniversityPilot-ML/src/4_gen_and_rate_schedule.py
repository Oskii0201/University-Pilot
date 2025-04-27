import pandas as pd

# 1. Wczytaj pliki
preliminary_df = pd.read_csv('DataInput/PreliminaryCoursesSchedule.csv')
generated_df = pd.read_csv('DataOutput/GeneratedSchedule.csv')
assigned_classrooms_df = pd.read_csv('DataOutput/assigned_classrooms.csv')

# 2. Połącz preliminary_df z generated_df
preliminary_df = preliminary_df.merge(
    generated_df[['course_id', 'start_datetime', 'end_datetime']],
    left_on='CourseScheduleId',
    right_on='course_id',
    how='left'
)

# 3. Zmień nazwy kolumn generated_df na NewDateTimeStart i NewDateTimeEnd
preliminary_df.rename(
    columns={
        'start_datetime': 'NewDateTimeStart',
        'end_datetime': 'NewDateTimeEnd'
    },
    inplace=True
)

# 4. Połącz preliminary_df z assigned_classrooms_df
preliminary_df = preliminary_df.merge(
    assigned_classrooms_df[['course_id', 'Sala_Przydzielona']],
    left_on='CourseScheduleId',
    right_on='course_id',
    how='left'
)

# 5. Ustaw ClassroomId i Classroom bez warunkowania typu zajęć
preliminary_df['ClassroomId'] = preliminary_df.apply(
    lambda row: row['Sala_Przydzielona'] if pd.notna(row['Sala_Przydzielona']) else "0",
    axis=1
)

preliminary_df['Classroom'] = preliminary_df.apply(
    lambda row: row['Sala_Przydzielona'] if pd.notna(row['Sala_Przydzielona']) else "",
    axis=1
)

# 6. Usuń pomocnicze kolumny
preliminary_df.drop(columns=['course_id_x', 'course_id_y', 'Sala_Przydzielona'], inplace=True)

# 7. Zapisz wynik
preliminary_df.to_csv('DataOutput/UpdatedPreliminaryCoursesSchedule.csv', index=False)

print("✅ Plik UpdatedPreliminaryCoursesSchedule.csv został utworzony!")
