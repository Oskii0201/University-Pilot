"use client";

import React, { useState, useEffect, useCallback } from "react";
import Select from "react-select";
import { Button } from "@/components/Button";
import GroupList from "@/components/schedule-builder/ScheduleGroupManagementForm/GroupList";
import UnassignedCourses from "@/components/schedule-builder/ScheduleGroupManagementForm/UnassignedCoursesList";
import { Course, Group, Semester } from "@/app/types";
import { toast } from "react-toastify";
import { useRouter } from "next/navigation";
import { handleApiError } from "@/utils/handleApiError";
import apiClient from "@/app/lib/apiClient";
import { fetchGroups } from "@/app/lib/api/fetchGroups";

const ScheduleGroupManagementForm: React.FC<{ semesterID?: number }> = ({
  semesterID,
}) => {
  const router = useRouter();
  const [semesters, setSemesters] = useState<Semester[]>([]);
  const [selectedSemester, setSelectedSemester] = useState<Semester | null>(
    null,
  );
  const [patternTitle, setPatternTitle] = useState("Grupy");
  const [groups, setGroups] = useState<Group[]>([]);
  const [unassignedCourses, setUnassignedCourses] = useState<Course[]>([]);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const fetchSemesters = async () => {
      try {
        const response = await apiClient.get(
          "/StudyProgram/GetUpcomingSemesters",
        );

        setSemesters(response.data);
      } catch (error) {
        toast.error(handleApiError(error));
      } finally {
        setIsLoading(false);
      }
    };
    fetchSemesters();
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
    const { groups, unassignedCourses } = await fetchGroups(semesterID);

    setUnassignedCourses(unassignedCourses);
    setGroups(groups);
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
      setPatternTitle(`Grupy ${semester?.name || ""}`);

      await fetchGroupAssignments(selectedOption.value);
      setIsLoading(false);
    },
    [groups, semesters, fetchGroupAssignments],
  );
  const handleEditGroupName = (groupId: number, newName: string) => {
    setGroups((prevGroups) =>
      prevGroups.map((g) =>
        g.groupId === groupId ? { ...g, groupName: newName } : g,
      ),
    );
  };
  const handleAddCourseToGroup = (groupId: number, courseName: string) => {
    const course = unassignedCourses.find((c) => c === courseName);
    if (!course) return;

    setGroups((prev) =>
      prev.map((g) =>
        g.groupId === groupId
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
        groupId:
          prev.length > 0 ? Math.max(...prev.map((g) => g.groupId)) + 1 : 1,
        groupName: `Grupa ${prev.length + 1}`,
        assignedFieldsOfStudy: [],
      },
    ]);
  };
  const handleRemoveCourseFromGroup = (groupId: number, courseName: string) => {
    const group = groups.find((g) => g.groupId === groupId);
    if (!group) return;

    const course = group.assignedFieldsOfStudy.find((c) => c === courseName);
    if (!course) return;

    setGroups((prev) =>
      prev.map((g) =>
        g.groupId === groupId
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
  const handleRemoveGroup = (groupId: number, courses: Course[]) => {
    setGroups((prev) => prev.filter((g) => g.groupId !== groupId));
    setUnassignedCourses((prev) => [...prev, ...courses]);
  };
  const handleSubmit = async () => {
    if (groups.length === 0) {
      return toast.error("Musisz stworzyć choć jedną grupę");
    }
    try {
      await apiClient.put(
        "/StudyProgram/UpdateFieldsOfStudyAssignmentsToGroup",
        {
          unassignedFieldsOfStudy: unassignedCourses,
          assignedFieldOfStudyGroups: groups,
        },
      );
      toast.success("Formularz wysłany pomyślnie!");
      router.push("/dashboard/schedule-builder/groups");
    } catch (error) {
      toast.error("Wystąpił błąd podczas wysyłania formularza.");
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
          <div className="flex flex-col gap-4">
            <label className="font-semibold" htmlFor="pattern-title">
              Nazwa zestawu:
            </label>
            <input
              id="pattern-title"
              type="text"
              value={patternTitle}
              onChange={(e) => setPatternTitle(e.target.value)}
              className="w-full rounded border p-2"
              placeholder="Wprowadź tytuł patternu"
            />
          </div>

          <div className="flex gap-4">
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
