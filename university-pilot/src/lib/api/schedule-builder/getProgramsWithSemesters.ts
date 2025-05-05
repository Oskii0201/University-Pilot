import apiClient from "@/lib/apiClient";
import { ProgramsWithSemesters } from "@/app/types";
import { handleApiError } from "@/utils/handleApiError";

/**
 * Pobiera wszystkie kierunki dla ponadego semestru wraz z semestrami
 * @param semesterId id semestru
 * @returns Promise z rezultatem zapytania (dane lub błąd)
 */
export const getProgramsWithSemesters = async (
  semesterId: number,
): Promise<{ data: ProgramsWithSemesters[] | null; error: string | null }> => {
  try {
    const response = await apiClient.get(
      `/Semester/${semesterId}/GetProgramsWithSemesters`,
    );
    return { data: response.data, error: null };
  } catch (error) {
    console.error("Błąd podczas pobierania semestrów:", error);
    return { data: null, error: handleApiError(error) };
  }
};
