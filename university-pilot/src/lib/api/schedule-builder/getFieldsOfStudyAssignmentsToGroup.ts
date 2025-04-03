import apiClient from "@/lib/apiClient";
import { FieldsOfStudyAssignmentsResponse } from "@/app/types";
import { handleApiError } from "@/utils/handleApiError";

/**
 * Pobiera przypisane i nieprzypisane kierunki studiów dla danego semestru
 * @param semesterId ID semestru
 * @returns Promise z rezultatem zapytania (dane lub błąd)
 */
export const getFieldsOfStudyAssignmentsToGroup = async (
  semesterId: number,
): Promise<{
  data: FieldsOfStudyAssignmentsResponse | null;
  error: string | null;
}> => {
  try {
    const response = await apiClient.get(
      `/Schedule/GetFieldsOfStudyAssignmentsToGroup?semesterId=${semesterId}`,
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
