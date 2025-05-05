import apiClient from "@/lib/apiClient";
import { Semester } from "@/app/types";
import { handleApiError } from "@/utils/handleApiError";

/**
 * Pobiera nadchodzące semestry po stage 2
 * @returns Promise z rezultatem zapytania (dane lub błąd)
 */
export const getUpcomingSemestersAfterGroupSchedule = async (): Promise<{
  data: Semester[] | null;
  error: string | null;
}> => {
  try {
    const response = await apiClient.get(
      "/Semester/GetSemestersWithStatusAfterGroupSchedule",
    );
    return { data: response.data, error: null };
  } catch (error) {
    console.error("Błąd podczas pobierania semestrów:", error);
    return { data: null, error: handleApiError(error) };
  }
};
