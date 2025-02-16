"use client";

import React, { useState, useEffect } from "react";
import Select from "react-select";
import { Button } from "@/components/Button";
import GroupList from "@/components/schedule-builder/ScheduleGroupManagementForm/GroupList";
import UnassignedCourses from "@/components/schedule-builder/ScheduleGroupManagementForm/UnassignedCoursesList";
import { Course, Group, Semester } from "@/app/types";
import axios from "axios";
import { toast } from "react-toastify";
import { useRouter } from "next/navigation";
import { handleApiError } from "@/utils/handleApiError";
import apiClient from "@/app/lib/apiClient";

const ScheduleGroupManagementForm: React.FC<{ groupID?: number }> = ({
  groupID,
}) => {
  const router = useRouter();
  const [semesters, setSemesters] = useState<Semester[]>([]);
  const [selectedSemester, setSelectedSemester] = useState<Semester | null>(
    null,
  );
  const [patternTitle, setPatternTitle] = useState<string>("Grupy");
  const [groups, setGroups] = useState<Group[]>([]);
  const [unassignedCourses, setUnassignedCourses] = useState<Course[]>([]);
  const [isLoading, setIsLoading] = useState<boolean>(true);

  useEffect(() => {
    const loadSemesters = async () => {
      setIsLoading(true);
      try {
        const response = await axios.get<Semester[]>(
          "/api/schedule-builder/groups/semesters",
        );
        setSemesters(response.data);
      } catch (error) {
        toast.error(handleApiError(error));
      } finally {
        setIsLoading(false);
      }
    };

    loadSemesters();
  }, []);

  useEffect(() => {
    if (groupID && semesters.length > 0) {
      handleSemesterChange({ value: Number(groupID) });
    }
  }, [groupID, semesters]);

  useEffect(() => {
    const handleBeforeUnload = (event: BeforeUnloadEvent) => {
      if (groups.length > 0 || unassignedCourses.length > 0) {
        event.preventDefault();
      }
    };

    window.addEventListener("beforeunload", handleBeforeUnload);
    return () => {
      window.removeEventListener("beforeunload", handleBeforeUnload);
    };
  }, [groups, unassignedCourses]);

  const fetchGetFieldsOfStudyAssignmentsToGroup = async (
    semesterID: number,
  ) => {
    try {
      setIsLoading(true);

      if (!semesterID) return;

      const response = await apiClient.get(
        `/StudyProgram/GetFieldsOfStudyAssignmentsToGroup?semesterId=${semesterID}`,
      );
      setUnassignedCourses(response.data.unassignedFieldsOfStudy);

      if (response.data.assignedFieldOfStudyGroups.length > 0) {
        setGroups(response.data.assignedFieldOfStudyGroups);
      }
    } catch (error) {
      toast.error("Nie udało się załadować kierunków.");
      console.error(error);
    } finally {
      setIsLoading(false);
    }
  };

  const handleSemesterChange = async (
    selectedOption: { value: number } | null,
  ) => {
    if (!selectedOption) return;
    setIsLoading(true);

    if (
      groups.length > 0 ||
      groups.some((group) => group.assignedFieldsOfStudy.length > 0)
    ) {
      const confirmChange = window.confirm(
        "Zmiana semestru spowoduje utratę wszystkich danych. Czy chcesz kontynuować?",
      );
      if (!confirmChange) return;
    }

    const selectedSemester = semesters.find(
      (semester) => semester.id === selectedOption.value,
    );

    setSelectedSemester(selectedSemester || null);

    setPatternTitle(`Grupy ${selectedSemester?.name}`);

    await fetchGetFieldsOfStudyAssignmentsToGroup(selectedOption.value);

    setIsLoading(false);
  };

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
  console.log(selectedSemester);
  return (
    <div className="grid grid-cols-1 gap-6 md:grid-cols-2">
      <div className="col-span-1 md:col-span-2">
        <h1 className="text-2xl font-bold">
          {groupID ? "Edytuj zestaw grup" : "Stwórz nowy zestaw grup"}
        </h1>
      </div>

      <div className="flex flex-col gap-4">
        <label className="font-semibold" htmlFor="semester">
          Wybierz semestr:
        </label>
        <Select
          instanceId="semester-select"
          value={
            selectedSemester && {
              value: selectedSemester.id,
              label: selectedSemester.name,
            }
          }
          options={semesters.map((semester) => ({
            value: semester.id,
            label: semester.name,
          }))}
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

          <Button
            onClick={() =>
              setGroups([
                ...groups,
                {
                  groupId:
                    groups.length > 0
                      ? Math.max(...groups.map((g) => g.groupId)) + 1
                      : 1,
                  groupName: `Grupa ${groups.length + 1}`,
                  assignedFieldsOfStudy: [],
                },
              ])
            }
            color="blue"
          >
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
