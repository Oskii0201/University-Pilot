import apiClient from "@/lib/apiClient";
import { handleApiError } from "@/utils/handleApiError";
import { Event, CalendarView } from "@/app/types";
interface FetchScheduleEventsParams {
  semesterId: number;
  name: string;
  semester: number;
  currentDate: string;
  viewType: CalendarView;
}

/**
 * Pobiera harmonogram na podstawie filtrów
 * @returns Promise z rezultatem zapytania (dane lub błąd)
 */
export const fetchScheduleEvents = async (
  props: FetchScheduleEventsParams,
): Promise<{
  data: Event[] | [];
  error: string | null;
}> => {
  try {
    const response = await apiClient.post("/Schedule/GetCalendar", props);

    return { data: response.data, error: null };
  } catch (error) {
    console.error("Błąd podczas pobierania semestrów:", error);
    return { data: [], error: handleApiError(error) };
  }
};
