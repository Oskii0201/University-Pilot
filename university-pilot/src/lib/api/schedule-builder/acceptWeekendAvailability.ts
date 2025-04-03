import apiClient from "@/lib/apiClient";
import { handleApiError } from "@/utils/handleApiError";

interface AcceptWeekendAvailabilityRequest {
  semesterId: number;
}

/**
 * Wysyła akceptacje dni zjazdowych do backendu i rozpoczyna generowanie harmonogramu wstępnego
 * @param payload dane do wysłania
 * @returns obiekt { success: boolean, error: string | null }
 */
export const acceptWeekendAvailability = async ({
  semesterId,
}: AcceptWeekendAvailabilityRequest): Promise<{
  success: boolean;
  error: string | null;
}> => {
  try {
    await apiClient.post(
      `/Schedule/AcceptWeekendAvailability?semesterId=${semesterId}`,
    );

    return { success: true, error: null };
  } catch (error) {
    console.error("Błąd podczas akceptacji:", error);
    return { success: false, error: handleApiError(error) };
  }
};
