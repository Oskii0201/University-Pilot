export interface Event {
  id: string; // Unikalny identyfikator
  title: string; // Tytuł wydarzenia
  description: string; // Opis wydarzenia
  startTime: string; // Data i czas rozpoczęcia w formacie ISO
  endTime: string; // Data i czas zakończenia w formacie ISO
}
