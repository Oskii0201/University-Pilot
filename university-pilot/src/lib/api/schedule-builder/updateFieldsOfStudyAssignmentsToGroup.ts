import apiClient from "@/lib/apiClient";
import { Group, Course } from "@/app/types";
import { handleApiError } from "@/utils/handleApiError";

interface UpdateFieldsOfStudyAssignmentsRequest {
  semesterId: number;
  unassignedFieldsOfStudy: Course[];
  assignedFieldOfStudyGroups: Omit<Group, "key">[];
}

/**
 * Wysyła zaktualizowane przypisania kierunków do grup
 * @param payload dane do wysłania
 * @returns obiekt { success: boolean, error: string | null }
 */
export const updateFieldsOfStudyAssignmentsToGroup = async (
  payload: UpdateFieldsOfStudyAssignmentsRequest,
): Promise<{ success: boolean; error: string | null }> => {
  try {
    await apiClient.put(
      "/Schedule/UpdateFieldsOfStudyAssignmentsToGroup",
      payload);
    return { success: true, error: null };
  } catch (error) {
    console.error("Błąd podczas aktualizacji przypisań:", error);
    return { success: false, error: handleApiError(error) };
  }
};
