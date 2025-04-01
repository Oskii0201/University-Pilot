import { toast } from "react-toastify";
import apiClient from "@/lib/apiClient";
import { Semester } from "@/app/types";
import { getRandomLoadingMessage } from "@/utils/getRandomLoadingMessage";

/**
 * Pobiera nadchodzące semestry
 * @returns Promise zawierający tablicę semestrów
 */
export const getUpcomingSemesters = async (): Promise<Semester[]> => {
  try {
    const response = await apiClient.get("/Schedule/GetUpcomingSemesters");
    return response.data;
  } catch (error) {
    toast.error(getRandomLoadingMessage("error"));
    console.error(error);
    return [];
  }
};
