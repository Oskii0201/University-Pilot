import apiClient from "@/app/lib/apiClient"; // Import instancji Axios
import events from "@/data/events.json"; // Tymczasowy import pliku JSON
import { Event } from "@/app/types";

/**
 * Funkcja do pobierania eventów.
 * Docelowo wysyła zapytanie do API z parametrami `od` i `do`.
 * Aktualnie filtruje dane lokalnie na potrzeby testów.
 *
 * @param startDate - Data początkowa zakresu (w formacie Date)
 * @param endDate - Data końcowa zakresu (w formacie Date)
 * @returns Lista eventów
 */
export const fetchEvents = async (
  startDate: Date,
  endDate: Date,
): Promise<Event[]> => {
  try {
    // Docelowe zapytanie do API
    // Ostateczna wersja - po podłączeniu API
    /*
    const response = await apiClient.get<Event[]>("/events", {
      params: {
        startDate: startDate.toISOString(),
        endDate: endDate.toISOString(),
      },
    });
    return response.data;
    */

    // Tymczasowe filtrowanie lokalne na potrzeby testów
    const filteredEvents = events.filter((event) => {
      const eventStart = new Date(event.startTime);
      const eventEnd = new Date(event.endTime);

      return eventStart <= endDate && eventEnd >= startDate;
    });

    // Zwracamy przefiltrowane dane z lokalnego pliku JSON
    return filteredEvents;
  } catch (error) {
    console.error("Błąd podczas pobierania eventów:", error);
    return [];
  }
};
