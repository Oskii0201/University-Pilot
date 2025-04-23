import apiClient from "@/lib/apiClient";
import { handleApiError } from "@/utils/handleApiError";
import Events from "@/app/data/testEvents.json";
import { CalendarView } from "@/app/types";

interface FetchScheduleEventsParams {
  semesterId: number;
  fieldOfStudyId: number;
  selectedSemesterNumber: number;
  currentDate: string;
  view: CalendarView;
}

/**
 * Pobiera harmonogram na podstawie filtrów
 * @param semesterId id semestru
 * @param fieldOfStudyId id kierunku
 * @param selectedSemesterNumber numer semestru
 * @param currentDate wybrana data
 * @param view typ widoku month | week
 * @returns Promise z rezultatem zapytania (dane lub błąd)
 */
export const fetchScheduleEvents = async ({
  semesterId,
  fieldOfStudyId,
  selectedSemesterNumber,
  currentDate,
  view,
}: FetchScheduleEventsParams): Promise<{
  data: Event[] | [];
  error: string | null;
}> => {
  try {
    // const response = await apiClient.get('/Calendar/GetEvents', {
    //   params: {
    //     semesterId,
    //   fieldOfStudyId,
    //   selectedSemesterNumber,
    //   currentDate,
    //   view,
    //   },
    // });

    return { data: Events as Event[], error: null };
  } catch (error) {
    console.error("Błąd podczas pobierania semestrów:", error);
    return { data: [], error: handleApiError(error) };
  }
};
