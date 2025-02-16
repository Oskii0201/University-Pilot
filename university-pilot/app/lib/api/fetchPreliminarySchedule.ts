import { toast } from "react-toastify";
import { BasicGroup, Weekend } from "@/app/types";
import { getRandomLoadingMessage } from "@/app/utils/getRandomLoadingMessage";

/**
 * Pobiera wstępny harmonogram wraz z grupami i weekendami
 * @param semesterID ID semestru
 * @returns Promise zawierający obiekt wstępnego harmonogramu wraz z grupami i weekendami
 */

const test_groups: BasicGroup[] = [
  { groupId: 1, groupName: "Grupa 1" },
  { groupId: 2, groupName: "Grupa 2" },
  { groupId: 3, groupName: "Grupa 3" },
  { groupId: 4, groupName: "Grupa 4" },
  { groupId: 5, groupName: "Grupa 5" },
];

const test_weekends: Weekend[] = [
  {
    date: "2024-03-02",
    availability: {
      1: false,
      2: false,
      3: false,
      4: false,
      5: false,
    },
  },
  {
    date: "2024-03-16",
    availability: {
      1: false,
      2: false,
      3: false,
      4: false,
      5: false,
    },
  },
  {
    date: "2024-04-06",
    availability: {
      1: false,
      2: false,
      3: false,
      4: false,
      5: false,
    },
  },
];

export const fetchPreliminarySchedule = async (
  semesterID: number,
): Promise<{
  semesterId: number;
  groups: BasicGroup[];
  weekends: Weekend[];
}> => {
  if (!semesterID) return { semesterId: 0, groups: [], weekends: [] };

  try {
    /*Kod finalny jak będzie endpoint*/
    /* const response = await apiClient.get(
      `/StudyProgram/[endpoint]?semesterId=${semesterID}`,
    );

    return {
      semesterId: response.data.semesterId,
      groups: response.data.assignedFieldOfStudyGroups,
      weekends: response.data.weekends,
    };*/

    /*Kod roboczy do testów*/
    return {
      semesterId: semesterID,
      groups: test_groups,
      weekends: test_weekends,
    };
  } catch (error) {
    toast.error(getRandomLoadingMessage("error"));
    console.error(error);
    return { semesterId: 0, groups: [], weekends: [] };
  }
};
