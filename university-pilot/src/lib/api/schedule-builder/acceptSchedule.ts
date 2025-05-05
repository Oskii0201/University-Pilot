import apiClient from "@/lib/apiClient";
import { handleApiError } from "@/utils/handleApiError";

interface AcceptScheduleRequest {
  semesterId: number;
}

/**
 * Wysyła akceptacje wgenerowanego harmonogramu
 * @param payload dane do wysłania
 * @returns obiekt { success: boolean, error: string | null }
 */
export const acceptSchedule = async ({
  semesterId,
}: AcceptScheduleRequest): Promise<{
  success: boolean;
  error: string | null;
}> => {
  try {
    await apiClient.get(`/Schedule/AcceptSchedule?semesterId=${semesterId}`);

    return { success: true, error: null };
  } catch (error) {
    console.error("Błąd podczas akceptacji:", error);
    return { success: false, error: handleApiError(error) };
  }
};
