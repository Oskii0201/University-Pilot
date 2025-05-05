import apiClient from "@/lib/apiClient";
import { handleApiError } from "@/utils/handleApiError";

/**
 * Pobiera aktualny status semestru
 * @param semesterId dane do wysłania
 * @returns Promise z rezultatem zapytania (dane lub błąd)
 */
export const getSemesterStatus = async (
  semesterId: number,
): Promise<{
  data: number | null;
  error: string | null;
}> => {
  try {
    const response = await apiClient.get(`/Semester/${semesterId}/Status`);

    return { data: Number(response.data), error: null };
  } catch (error) {
    console.error("Błąd podczas pobierania statusu:", error);
    return {
      data: null,
      error: handleApiError(error),
    };
  }
};
