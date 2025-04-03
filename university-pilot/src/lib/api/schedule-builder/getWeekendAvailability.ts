import apiClient from "@/lib/apiClient";
import { WeekendAvailabilityResponse } from "@/app/types";
import { handleApiError } from "@/utils/handleApiError";

/**
 * Pobiera dostępność w weekendy
 * @param semesterId ID semestru
 * @returns Promise z rezultatem zapytania (dane lub błąd)
 */
export const getWeekendAvailability = async (
  semesterId: number,
): Promise<{
  data: WeekendAvailabilityResponse | null;
  error: string | null;
}> => {
  try {
    const response = await apiClient.get(
      `/Schedule/GetWeekendAvailability?semesterId=${semesterId}`,
    );
    return { data: response.data, error: null };
  } catch (error) {
    console.error("Błąd podczas pobierania danych przypisań:", error);
    return {
      data: null,
      error: handleApiError(error),
    };
  }
};
