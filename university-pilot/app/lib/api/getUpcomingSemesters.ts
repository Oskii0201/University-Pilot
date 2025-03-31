import apiClient from "@/app/lib/apiClient";
import { toast } from "react-toastify";
import { Semester } from "@/app/types";
import { getRandomLoadingMessage } from "@/app/utils/getRandomLoadingMessage";

/**
 * Pobiera nadchodzące semestry
 * @returns Promise zawierający tablicę semestrów
 */
export const getUpcomingSemesters = async (): Promise<Semester[]> => {
  try {
    const response = await apiClient.get("/StudyProgram/GetUpcomingSemesters");
    return response.data;
  } catch (error) {
    toast.error(getRandomLoadingMessage("error"));
    console.error(error);
    return [];
  }
};
