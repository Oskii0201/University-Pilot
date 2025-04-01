"use client";

import React, { useState, useEffect, useCallback } from "react";
import Select from "react-select";
import { Button } from "@/components/ui/Button";
import GroupList from "@/components/schedule-builder/ScheduleGroupManagementForm/GroupList";
import UnassignedCourses from "@/components/schedule-builder/ScheduleGroupManagementForm/UnassignedCoursesList";
import { Course, Group, Semester } from "@/app/types";
import { toast } from "react-toastify";
import { useRouter } from "next/navigation";
import apiClient from "@/lib/apiClient";
import { v4 as uuidv4 } from "uuid";
import { getRandomLoadingMessage } from "@/utils/getRandomLoadingMessage";
import { getUpcomingSemesters } from "@/lib/api/getUpcomingSemesters";
import { getFieldsOfStudyAssignmentsToGroup } from "@/lib/api/getFieldsOfStudyAssignmentsToGroup";

const ScheduleGroupManagementForm: React.FC<{ semesterID?: number }> = ({
  semesterID,
}) => {
  const router = useRouter();
  const [semesters, setSemesters] = useState<Semester[]>([]);
  const [selectedSemester, setSelectedSemester] = useState<Semester | null>(
    null,
  );
  const [groups, setGroups] = useState<Group[]>([]);
  const [unassignedCourses, setUnassignedCourses] = useState<Course[]>([]);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const loadSemesters = async () => {
      setIsLoading(true);
      const data = await getUpcomingSemesters();
      setSemesters(data);
      setIsLoading(false);
    };
    loadSemesters();
  }, []);

  useEffect(() => {
    if (semesterID && semesters.length > 0) {
      handleSemesterChange({ value: Number(semesterID) });
    }
  }, [semesterID, semesters]);

  useEffect(() => {
    const handleBeforeUnload = (event: BeforeUnloadEvent) => {
      if (groups.length > 0 || unassignedCourses.length > 0) {
        event.preventDefault();
      }
    };

    window.addEventListener("beforeunload", handleBeforeUnload);
    return () => window.removeEventListener("beforeunload", handleBeforeUnload);
  }, [groups, unassignedCourses]);

  const fetchGroupAssignments = useCallback(async (semesterID: number) => {
    setIsLoading(true);
    const { groups, unassignedCourses } =
      await getFieldsOfStudyAssignmentsToGroup(semesterID);

    const updatedGroups = groups.map((group) => ({
      ...group,
      key: uuidv4(),
    }));

    setUnassignedCourses(unassignedCourses);
    setGroups(updatedGroups);
  }, []);

  const handleSemesterChange = useCallback(
    async (selectedOption: { value: number } | null) => {
      if (!selectedOption) return;
      setIsLoading(true);

      if (
        groups.length > 0 ||
        groups.some((g) => g.assignedFieldsOfStudy.length > 0)
      ) {
        if (
          !window.confirm(
            "Zmiana semestru spowoduje utratę wszystkich danych. Kontynuować?",
          )
        ) {
          setIsLoading(false);
          return;
        }
      }

      const semester =
        semesters.find((s) => s.id === selectedOption.value) || null;
      setSelectedSemester(semester);

      await fetchGroupAssignments(selectedOption.value);
      setIsLoading(false);
    },
    [groups, semesters, fetchGroupAssignments],
  );
  const handleEditGroupName = (key: string, newName: string) => {
    setGroups((prevGroups) =>
      prevGroups.map((g) => (g.key === key ? { ...g, groupName: newName } : g)),
    );
  };
  const handleAddCourseToGroup = (key: string, courseName: string) => {
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
  };
  const handleAddGroup = () => {
    setGroups((prev) => [
      ...prev,
      {
        key: uuidv4(),
        groupId: 0,
        groupName: `Grupa ${prev.length + 1}`,
        assignedFieldsOfStudy: [],
      },
    ]);
  };
  const handleRemoveCourseFromGroup = (key: string, courseName: string) => {
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
  };
  const handleRemoveGroup = (key: string, courses: Course[]) => {
    setGroups((prev) => prev.filter((g) => g.key !== key));
    setUnassignedCourses((prev) => [...prev, ...courses]);
  };
  const handleSubmit = async () => {
    if (groups.length === 0) {
      return toast.error("Musisz stworzyć choć jedną grupę");
    }

    try {
      if (!selectedSemester) return;
      await apiClient.put(
        "/StudyProgram/UpdateFieldsOfStudyAssignmentsToGroup",
        {
          semesterId: selectedSemester.id,
          unassignedFieldsOfStudy: unassignedCourses,
          assignedFieldOfStudyGroups: groups,
        },
      );
      toast.success(getRandomLoadingMessage("success"));
      router.push(`/dashboard/schedule-builder/groups/${selectedSemester.id}`);
    } catch (error) {
      toast.error(getRandomLoadingMessage("error"));
      console.error(error);
    }
  };

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
              {groups.reduce(
                (sum, group) => sum + group.assignedFieldsOfStudy.length,
                0,
              )}{" "}
              / {unassignedCourses.length}
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

          <Button onClick={handleAddGroup} color="blue">
            Dodaj grupę
          </Button>

          <Button onClick={handleSubmit} color="green">
            Wyślij formularz
          </Button>
        </>
      )}
    </div>
  );
};

export default ScheduleGroupManagementForm;
