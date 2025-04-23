import apiClient from "@/lib/apiClient";
import { FieldOfStudy } from "@/app/types";
import { handleApiError } from "@/utils/handleApiError";
import FieldsOfStudy from "@/app/data/testFieldsOfStudy.json";

/**
 * Pobiera wszystkie kierunki dla ponadego semestru
 * @param semesterId id semestru
 * @returns Promise z rezultatem zapytania (dane lub błąd)
 */
export const getFieldsOfStudy = async (
  semesterId: number,
): Promise<{ data: FieldOfStudy[] | null; error: string | null }> => {
  try {
    const response = await apiClient.get(
      `/Semester/GetSemestersByStatus/${semesterId}`,
    );
    /*return { data: response.data, error: null };*/
    return { data: FieldsOfStudy, error: null };
  } catch (error) {
    console.error("Błąd podczas pobierania semestrów:", error);
    return { data: null, error: handleApiError(error) };
  }
};
