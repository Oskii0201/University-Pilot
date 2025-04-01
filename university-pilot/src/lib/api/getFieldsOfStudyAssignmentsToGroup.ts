import { toast } from "react-toastify";
import { Group, Course } from "@/app/types";
import apiClient from "@/lib/apiClient";
import { getRandomLoadingMessage } from "@/utils/getRandomLoadingMessage";

/**
 * Pobiera przypisane i nieprzypisane kierunki dla podanego semestru
 * @param semesterID ID semestru
 * @returns Promise zawierający obiekt z grupami i kursami
 */
export const getFieldsOfStudyAssignmentsToGroup = async (
  semesterID: number,
): Promise<{
  groups: Group[];
  unassignedCourses: Course[];
}> => {
  if (!semesterID) return { groups: [], unassignedCourses: [] };

  try {
    const response = await apiClient.get(
      `/Schedule/GetFieldsOfStudyAssignmentsToGroup?semesterId=${semesterID}`,
    );

    return {
      groups: response.data.assignedFieldOfStudyGroups || [],
      unassignedCourses: response.data.unassignedFieldsOfStudy || [],
    };
  } catch (error) {
    toast.error(getRandomLoadingMessage("error"));
    console.error(error);
    return { groups: [], unassignedCourses: [] };
  }
};
