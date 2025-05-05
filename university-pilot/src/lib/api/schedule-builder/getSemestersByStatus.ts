import apiClient from "@/lib/apiClient";
import { Semester } from "@/app/types";
import { handleApiError } from "@/utils/handleApiError";

/**
 * Pobiera semestry na podstawie statusu
 * @param status status semestru
 * @returns Promise z rezultatem zapytania (dane lub błąd)
 */
export const getSemestersByStatus = async (
  status: number = 0,
): Promise<{ data: Semester[] | null; error: string | null }> => {
  try {
    const response = await apiClient.get(
      `/Semester/GetSemestersByStatus/${status}`,
    );
    return { data: response.data, error: null };
  } catch (error) {
    console.error("Błąd podczas pobierania semestrów:", error);
    return { data: null, error: handleApiError(error) };
  }
};
