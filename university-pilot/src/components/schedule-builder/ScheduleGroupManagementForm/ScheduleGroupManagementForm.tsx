"use client";

import React, { useState, useEffect, useCallback } from "react";
import Select from "react-select";
import { Button } from "@/components/ui/Button";
import GroupList from "@/components/schedule-builder/ScheduleGroupManagementForm/GroupList";
import UnassignedCourses from "@/components/schedule-builder/ScheduleGroupManagementForm/UnassignedCoursesList";
import { Course, Group, Semester } from "@/app/types";
import { toast } from "react-toastify";
import { useRouter } from "next/navigation";
import { v4 as uuidv4 } from "uuid";
import { getRandomLoadingMessage } from "@/utils/getRandomLoadingMessage";
import { getUpcomingSemesters } from "@/lib/api/schedule-builder/getUpcomingSemesters";
import { getFieldsOfStudyAssignmentsToGroup } from "@/lib/api/schedule-builder/getFieldsOfStudyAssignmentsToGroup";
import { updateFieldsOfStudyAssignmentsToGroup } from "@/lib/api/schedule-builder/updateFieldsOfStudyAssignmentsToGroup";

interface ScheduleGroupManagementFormProps {
  semesterID?: number;
}

const ScheduleGroupManagementForm: React.FC<
  ScheduleGroupManagementFormProps
> = ({ semesterID }) => {
  const router = useRouter();

  // State
  const [semesters, setSemesters] = useState<Semester[]>([]);
  const [selectedSemester, setSelectedSemester] = useState<Semester | null>(
    null,
  );
  const [groups, setGroups] = useState<Group[]>([]);
  const [unassignedCourses, setUnassignedCourses] = useState<Course[]>([]);
  const [isLoading, setIsLoading] = useState(true);

  // Helpers
  const hasUnsavedChanges = groups.length > 0 || unassignedCourses.length > 0;
  const totalAssignedCourses = groups.reduce(
    (sum, group) => sum + group.assignedFieldsOfStudy.length,
    0,
  );
  const totalCourses = totalAssignedCourses + unassignedCourses.length;

  // Data fetching
  const fetchSemesters = useCallback(async () => {
    try {
      setIsLoading(true);
      const { data, error } = await getUpcomingSemesters(0);
      if (error || !data) {
        toast.error("Nie udało się pobrać semestrów");
        return;
      }
      setSemesters(data);
    } catch (error) {
      toast.error("Wystąpił błąd przy pobieraniu semestrów");
      console.error(error);
    } finally {
      setIsLoading(false);
    }
  }, []);

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

      setGroups(updatedGroups);
      setUnassignedCourses(data.unassignedFieldsOfStudy);
    } catch (error) {
      toast.error("Wystąpił błąd przy pobieraniu przypisań grup");
      console.error(error);
    } finally {
      setIsLoading(false);
    }
  }, []);

  // Event handlers
  const handleSemesterChange = useCallback(
    async (selectedOption: { value: number } | null) => {
      if (!selectedOption) return;

      if (hasUnsavedChanges) {
        if (
          !window.confirm(
            "Zmiana semestru spowoduje utratę wszystkich danych. Kontynuować?",
          )
        ) {
          return;
        }
      }

      const semester =
        semesters.find((s) => s.id === selectedOption.value) || null;
      setSelectedSemester(semester);
      await fetchGroupAssignments(selectedOption.value);
    },
    [hasUnsavedChanges, semesters, fetchGroupAssignments],
  );

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

  const handleSubmit = async () => {
    if (!selectedSemester) {
      return toast.error("Nie wybrano semestru");
    }

    if (groups.length === 0) {
      return toast.error("Musisz stworzyć choć jedną grupę");
    }

    try {
      setIsLoading(true);

      const payload = {
        semesterId: selectedSemester.id,
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
      router.push(`/dashboard/schedule-builder/groups/${selectedSemester.id}`);
    } catch (error) {
      toast.error(getRandomLoadingMessage("error"));
      console.error(error);
    } finally {
      setIsLoading(false);
    }
  };

  // Effects
  useEffect(() => {
    fetchSemesters();
  }, [fetchSemesters]);

  useEffect(() => {
    if (semesterID && semesters.length > 0) {
      handleSemesterChange({ value: Number(semesterID) });
    }
  }, [semesterID, semesters, handleSemesterChange]);

  useEffect(() => {
    const handleBeforeUnload = (event: BeforeUnloadEvent) => {
      if (hasUnsavedChanges) {
        event.preventDefault();
        return (event.returnValue = "");
      }
    };

    window.addEventListener("beforeunload", handleBeforeUnload);
    return () => window.removeEventListener("beforeunload", handleBeforeUnload);
  }, [hasUnsavedChanges]);

  // Render
  return (
    <div className="grid grid-cols-1 gap-6 md:grid-cols-2">
      <div className="col-span-1 md:col-span-2">
        <h1 className="text-2xl font-bold">
          {semesterID ? "Edytuj zestaw grup" : "Stwórz nowy zestaw grup"}
        </h1>
      </div>

      <div className="flex flex-col gap-4">
        <label className="font-semibold" htmlFor="semester">
          Wybierz semestr:
        </label>
        <Select
          instanceId="semester-select"
          value={
            selectedSemester
              ? { value: selectedSemester.id, label: selectedSemester.name }
              : null
          }
          options={semesters.map((s) => ({ value: s.id, label: s.name }))}
          onChange={handleSemesterChange}
          isSearchable
          placeholder="Wybierz semestr..."
          isLoading={isLoading}
        />
      </div>

      {selectedSemester && (
        <>
          <div className="flex items-center justify-center gap-4">
            Przypisane kierunki:{" "}
            <span className="font-bold">
              {totalAssignedCourses} / {totalCourses}
            </span>
          </div>

          <UnassignedCourses unassignedCourses={unassignedCourses} />

          <GroupList
            groups={groups}
            unassignedCourses={unassignedCourses}
            handleEditGroupName={handleEditGroupName}
            handleRemoveCourseFromGroup={handleRemoveCourseFromGroup}
            handleAddCourseToGroup={handleAddCourseToGroup}
            handleRemoveGroup={handleRemoveGroup}
          />

          <div className="col-span-1 flex flex-col gap-4 md:col-span-2">
            <div className="grid grid-cols-1 gap-4 md:grid-cols-2">
              <Button
                onClick={handleAddGroup}
                color="blue"
                disabled={isLoading}
              >
                Dodaj grupę
              </Button>

              <Button onClick={handleSubmit} color="green" disabled={isLoading}>
                {isLoading ? "Zapisywanie..." : "Wyślij formularz"}
              </Button>
            </div>
          </div>
        </>
      )}
    </div>
  );
};

export default ScheduleGroupManagementForm;
