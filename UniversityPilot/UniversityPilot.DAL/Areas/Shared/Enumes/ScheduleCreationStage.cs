namespace UniversityPilot.DAL.Areas.Shared.Enumes
{
    public enum ScheduleCreationStage
    {
        New = 0, // Tworzenie grup pod określenie dni zjazdu
        GroupsScheduleCreating = 1, // Tworzenie grup zjazdowych dla danego semestru
        GroupsScheduleCreated = 2, // Określanie dni zjazdu
        GeneratingPreliminarySchedule = 3, // Akceptacja dni zjazdów i przesłanie na tworzenie wstępnego hrmonogramu
        GeneratingSchedule = 4, // Tworzenie harmonogramu poprzez AI
        GeneratedSchedule = 5, // Utworzony harmonogram AI
        ApprovedSchedule = 6 // Zaakceptowany harmonogram
    }
}