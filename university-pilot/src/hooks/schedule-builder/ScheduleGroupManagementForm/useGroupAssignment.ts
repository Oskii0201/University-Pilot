import { useState, useCallback, useEffect } from "react";
import { toast } from "react-toastify";
import { v4 as uuidv4 } from "uuid";
import { Course, Group } from "@/app/types";
import { getFieldsOfStudyAssignmentsToGroup } from "@/lib/api/schedule-builder/getFieldsOfStudyAssignmentsToGroup";
import { updateFieldsOfStudyAssignmentsToGroup } from "@/lib/api/schedule-builder/updateFieldsOfStudyAssignmentsToGroup";
import { getRandomLoadingMessage } from "@/utils/getRandomLoadingMessage";
import { useRouter } from "next/navigation";

export const useGroupAssignments = (initialSemesterID?: number) => {
  const router = useRouter();
  const [groups, setGroups] = useState<Group[]>([]);
  const [unassignedCourses, setUnassignedCourses] = useState<Course[]>([]);
  const [isLoading, setIsLoading] = useState(false);
  const [semesterName, setSemesterName] = useState<string>();

  const fetchGroupAssignments = useCallback(async (semesterID: number) => {
    try {
      setIsLoading(true);
      const { data, error } =
        await getFieldsOfStudyAssignmentsToGroup(semesterID);

      if (error || !data) {
        toast.error("Nie udało się pobrać przypisań grup");
        return;
      }

      const updatedGroups: Group[] = data.assignedFieldOfStudyGroups.map(
        (group) => ({
          ...group,
          key: uuidv4(),
        }),
      );
      setSemesterName(data.name);
      setGroups(updatedGroups);
      setUnassignedCourses(data.unassignedFieldsOfStudy);
    } catch (error) {
      toast.error("Wystąpił błąd przy pobieraniu przypisań grup");
      console.error(error);
    } finally {
      setIsLoading(false);
    }
  }, []);

  useEffect(() => {
    if (initialSemesterID) {
      fetchGroupAssignments(initialSemesterID);
    }
  }, [initialSemesterID, fetchGroupAssignments]);

  const handleEditGroupName = useCallback((key: string, newName: string) => {
    setGroups((prevGroups) =>
      prevGroups.map((g) => (g.key === key ? { ...g, groupName: newName } : g)),
    );
  }, []);

  const handleAddCourseToGroup = useCallback(
    (key: string, courseName: string) => {
      const course = unassignedCourses.find((c) => c === courseName);
      if (!course) return;

      setGroups((prev) =>
        prev.map((g) =>
          g.key === key
            ? {
                ...g,
                assignedFieldsOfStudy: [...g.assignedFieldsOfStudy, course],
              }
            : g,
        ),
      );

      setUnassignedCourses((prev) => prev.filter((c) => c !== courseName));
    },
    [unassignedCourses],
  );

  const handleRemoveCourseFromGroup = useCallback(
    (key: string, courseName: string) => {
      const group = groups.find((g) => g.key === key);
      if (!group) return;

      const course = group.assignedFieldsOfStudy.find((c) => c === courseName);
      if (!course) return;

      setGroups((prev) =>
        prev.map((g) =>
          g.key === key
            ? {
                ...g,
                assignedFieldsOfStudy: g.assignedFieldsOfStudy.filter(
                  (c) => c !== courseName,
                ),
              }
            : g,
        ),
      );

      setUnassignedCourses((prev) => [...prev, course]);
    },
    [groups],
  );

  const handleAddGroup = useCallback(() => {
    setGroups((prev) => [
      ...prev,
      {
        key: uuidv4(),
        groupId: 0,
        groupName: `Grupa ${prev.length + 1}`,
        assignedFieldsOfStudy: [],
      },
    ]);
  }, []);

  const handleRemoveGroup = useCallback((key: string, courses: Course[]) => {
    setGroups((prev) => prev.filter((g) => g.key !== key));
    setUnassignedCourses((prev) => [...prev, ...courses]);
  }, []);

  const handleSubmit = async (semesterID: number) => {
    if (!semesterID) {
      return toast.error("Nie wybrano semestru");
    }

    if (groups.length === 0) {
      return toast.error("Musisz stworzyć choć jedną grupę");
    }

    if (unassignedCourses.length > 0) {
      return toast.error("Musisz przypisać wszystkie kursy");
    }

    try {
      setIsLoading(true);

      const payload = {
        semesterId: semesterID,
        name: semesterName,
        unassignedFieldsOfStudy: unassignedCourses,
        assignedFieldOfStudyGroups: groups,
      };

      const { success, error } =
        await updateFieldsOfStudyAssignmentsToGroup(payload);

      if (!success) {
        toast.error(error ?? "Nie udało się zapisać danych.");
        return;
      }

      toast.success(getRandomLoadingMessage("success"));
      router.push(`/dashboard/schedule-builder/groups/${semesterID}`);
    } catch (error) {
      toast.error(getRandomLoadingMessage("error"));
      console.error(error);
    } finally {
      setIsLoading(false);
    }
  };

  // Helpers
  const getTotalAssignedCourses = () => {
    return groups.reduce(
      (sum, group) => sum + group.assignedFieldsOfStudy.length,
      0,
    );
  };

  const getTotalCourses = () => {
    return getTotalAssignedCourses() + unassignedCourses.length;
  };

  const hasUnsavedChanges = groups.length > 0 || unassignedCourses.length > 0;

  return {
    groups,
    unassignedCourses,
    isLoading,
    hasUnsavedChanges,
    fetchGroupAssignments,
    handleEditGroupName,
    handleAddCourseToGroup,
    handleRemoveCourseFromGroup,
    handleAddGroup,
    handleRemoveGroup,
    handleSubmit,
    getTotalAssignedCourses,
    getTotalCourses,
  };
};
