import apiClient from "@/lib/apiClient";
import { BasicGroup, Weekend } from "@/app/types";
import { handleApiError } from "@/utils/handleApiError";

interface UpdateWeekendAvailabilityRequest {
  semesterId: number;
  groups: BasicGroup[];
  weekends: Weekend[];
}

/**
 * Wysyła zaktualizowane przypisania kierunków do grup
 * @param payload dane do wysłania
 * @returns obiekt { success: boolean, error: string | null }
 */
export const updateWeekendAvailability = async (
  payload: UpdateWeekendAvailabilityRequest,
): Promise<{ success: boolean; error: string | null }> => {
  try {
    await apiClient.put(
      "/Schedule/SaveWeekendAvailability", 
      payload);
    return { success: true, error: null };
  } catch (error) {
    console.error("Błąd podczas aktualizacji przypisań:", error);
    return { success: false, error: handleApiError(error) };
  }
};
