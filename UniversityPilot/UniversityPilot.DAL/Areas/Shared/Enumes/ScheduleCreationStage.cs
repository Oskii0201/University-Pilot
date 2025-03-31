namespace UniversityPilot.DAL.Areas.Shared.Enumes
{
    public enum ScheduleCreationStage
    {
        New = 0, // Tworzenie grup pod określenie dni zjazdu
        GroupsScheduleCreated = 1, // Określenie dni zjazdu
        GeneratingPreliminarySchedule = 2, // Akceptacja zjazdów i przesłanie na tworzenie wstępnego hrmonogramu
        PreliminarySchedule = 3, // Utworzenie wstępnego hrmonogramu
        GeneratingSchedule = 4, // Tworzenie harmonogramu poprzez AI
        GeneratedSchedule = 5, // Utworzony harmonogram AI
        ApprovedSchedule = 6 // Zaakceptowany harmonogram
    }
}