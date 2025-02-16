import apiClient from "@/app/lib/apiClient";
import { toast } from "react-toastify";
import { Group, Course } from "@/app/types";

/**
 * Pobiera przypisane i nieprzypisane kierunki dla podanego semestru
 * @param semesterID ID semestru
 * @returns Promise zawierający obiekt z grupami i kursami
 */
export const fetchGroups = async (
  semesterID: number,
): Promise<{
  groups: Group[];
  unassignedCourses: Course[];
}> => {
  if (!semesterID) return { groups: [], unassignedCourses: [] };

  try {
    const response = await apiClient.get(
      `/StudyProgram/GetFieldsOfStudyAssignmentsToGroup?semesterId=${semesterID}`,
    );

    return {
      groups: response.data.assignedFieldOfStudyGroups || [],
      unassignedCourses: response.data.unassignedFieldsOfStudy || [],
    };
  } catch (error) {
    toast.error("Nie udało się załadować kierunków.");
    console.error(error);
    return { groups: [], unassignedCourses: [] };
  }
};
