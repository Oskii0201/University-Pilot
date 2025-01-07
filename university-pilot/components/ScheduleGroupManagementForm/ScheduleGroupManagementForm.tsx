"use client";

import React, { useState, useEffect } from "react";
import Select from "react-select";
import { Button } from "@/components/Button";
import GroupList from "@/components/ScheduleGroupManagementForm/GroupList";
import UnassignedCourses from "@/components/ScheduleGroupManagementForm/UnassignedCoursesList";
import { v4 as uuidv4 } from "uuid";
import { Course, Group, Semester } from "@/app/types";
import axios from "axios";
import { toast } from "react-toastify";

const ScheduleGroupManagementForm: React.FC = () => {
  const [semesters, setSemesters] = useState<Semester[]>([]);
  const [selectedSemester, setSelectedSemester] = useState<string | null>(null);
  const [patternTitle, setPatternTitle] = useState<string>("Grupy");
  const [groups, setGroups] = useState<Group[]>([]);
  const [unassignedCourses, setUnassignedCourses] = useState<Course[]>([]);
  const [isLoading, setIsLoading] = useState<boolean>(true);

  const fetchData = async <T,>(url: string): Promise<T | null> => {
    try {
      const response = await axios.get<T>(url);
      return response.data;
    } catch (error) {
      toast.error("Wystąpił błąd podczas pobierania danych.");
      console.error(error);
      return null;
    }
  };

  useEffect(() => {
    const loadSemesters = async () => {
      setIsLoading(true);
      const data = await fetchData<Semester[]>("/api/semesters");
      if (data) setSemesters(data);
      setIsLoading(false);
    };

    loadSemesters();
  }, []);

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

  const handleSemesterChange = async (
    selectedOption: { value: string } | null,
  ) => {
    if (!selectedOption) return;

    if (groups.length > 0 || groups.some((group) => group.courses.length > 0)) {
      const confirmChange = window.confirm(
        "Zmiana semestru spowoduje utratę wszystkich danych. Czy chcesz kontynuować?",
      );
      if (!confirmChange) return;
    }

    setSelectedSemester(selectedOption.value);

    const selectedSemester = semesters.find(
      (semester) => semester.id === selectedOption.value,
    );
    setPatternTitle(
      `Grupy ${selectedSemester?.name || ""} ${selectedSemester?.academicYear || ""}`,
    );

    setGroups([]);
    setUnassignedCourses([]);
    setIsLoading(true);

    const data = await fetchData<Course[]>(
      `/api/courses?semesterId=${selectedOption.value}`,
    );
    if (data) setUnassignedCourses(data);

    setIsLoading(false);
  };

  const handleEditGroupName = (groupId: string, newName: string) => {
    setGroups((prevGroups) =>
      prevGroups.map((g) => (g.id === groupId ? { ...g, name: newName } : g)),
    );
  };

  const handleAddCourseToGroup = (groupId: string, courseId: number) => {
    const course = unassignedCourses.find((c) => c.id === courseId);
    if (!course) return;

    setGroups((prev) =>
      prev.map((g) =>
        g.id === groupId ? { ...g, courses: [...g.courses, course] } : g,
      ),
    );

    setUnassignedCourses((prev) => prev.filter((c) => c.id !== courseId));
  };

  const handleRemoveCourseFromGroup = (groupId: string, courseId: number) => {
    const group = groups.find((g) => g.id === groupId);
    if (!group) return;

    const course = group.courses.find((c) => c.id === courseId);
    if (!course) return;

    setGroups((prev) =>
      prev.map((g) =>
        g.id === groupId
          ? {
              ...g,
              courses: g.courses.filter((c) => c.id !== courseId),
            }
          : g,
      ),
    );

    setUnassignedCourses((prev) => [...prev, course]);
  };

  const handleRemoveGroup = (groupId: string, courses: Course[]) => {
    setGroups((prev) => prev.filter((g) => g.id !== groupId));
    setUnassignedCourses((prev) => [...prev, ...courses]);
  };

  const handleSubmit = async () => {
    if (unassignedCourses.length > 0) {
      toast.error(
        "Musisz przypisać wszystkie kierunki przed wysłaniem formularza.",
      );
      return;
    }

    const data = {
      semesterId: selectedSemester,
      patternTitle,
      groups: groups.map((group) => ({
        name: group.name,
        courses: group.courses.map((course) => course.id),
      })),
    };

    try {
      const response = await axios.post("/api/groups", data);
      if (response.status === 200) {
        toast.success("Formularz wysłany pomyślnie!");
      } else {
        throw new Error("Błąd podczas wysyłania danych");
      }
    } catch (error) {
      toast.error("Wystąpił błąd podczas wysyłania formularza.");
      console.error(error);
    }
  };

  return (
    <div className="grid grid-cols-1 gap-6 md:grid-cols-2">
      <div className="col-span-1 md:col-span-2">
        <h1 className="text-2xl font-bold">
          Zarządzanie grupami harmonogramowymi
        </h1>
      </div>

      <div className="flex flex-col gap-4">
        <label className="font-semibold" htmlFor="semester">
          Wybierz semestr:
        </label>
        <Select
          instanceId="semester-select"
          options={semesters.map((semester) => ({
            value: semester.id,
            label: `${semester.name} ${semester.academicYear}`,
          }))}
          onChange={(newValue) => handleSemesterChange(newValue)}
          isSearchable
          placeholder="Wybierz semestr..."
          isLoading={isLoading}
        />
      </div>

      {selectedSemester && (
        <>
          <div className="flex flex-col gap-4">
            <label className="font-semibold" htmlFor="pattern-title">
              Tytuł patternu:
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
              {groups.reduce((sum, group) => sum + group.courses.length, 0)} /{" "}
              {unassignedCourses.length}
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
                  id: uuidv4(),
                  name: `Grupa ${groups.length + 1}`,
                  courses: [],
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
