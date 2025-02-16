"use client";

import React, { useState, useEffect, useCallback } from "react";
import Select from "react-select";
import DataTable from "react-data-table-component";
import { Button } from "@/components/Button";
import { BasicGroup, Semester, Weekend } from "@/app/types";
import { fetchPreliminarySchedule } from "@/app/lib/api/fetchPreliminarySchedule";
import { fetchUpcomingSemesters } from "@/app/lib/api/fetchUpcomingSemesters";

/**
 * Sprawdza, czy w podanym harmonogramie (schedule) znajduje się jakakolwiek wartość `true` w `availability`.
 * @param schedule - Tablica obiektów z datami i dostępnościami
 * @returns `true`, jeśli co najmniej jedna wartość `true` jest w obiekcie `availability`, w przeciwnym razie `false`.
 */
const hasTrueValues = (schedule: Weekend[]) =>
  schedule.some(({ availability }) =>
    Object.values(availability).some(Boolean),
  );

const PreliminaryScheduleForm: React.FC<{
  semesterID?: number;
  readOnlyMode?: boolean;
}> = ({ semesterID, readOnlyMode = false }) => {
  const [semesters, setSemesters] = useState<Semester[]>([]);
  const [selectedSemester, setSelectedSemester] = useState<Semester | null>(
    null,
  );
  const [schedule, setSchedule] = useState<Weekend[]>([]);
  const [groups, setGroups] = useState<BasicGroup[]>([]);
  const [isLoading, setIsLoading] = useState(false);

  useEffect(() => {
    const loadSemesters = async () => {
      setIsLoading(true);
      const data = await fetchUpcomingSemesters();
      setSemesters(data);
      setIsLoading(false);
    };
    loadSemesters();
  }, []);

  const handleSemesterChange = useCallback(
    async (selectedOption: { value: number } | null) => {
      if (!selectedOption) return;
      setIsLoading(true);

      if (!readOnlyMode && hasTrueValues(schedule)) {
        const confirmChange = window.confirm(
          "Zmiana semestru spowoduje utratę wszystkich danych. Kontynuować?",
        );
        if (!confirmChange) {
          setIsLoading(false);
          return;
        }
      }

      const semester =
        semesters.find((s) => s.id === selectedOption.value) ?? null;
      setSelectedSemester(semester);

      const data = await fetchPreliminarySchedule(selectedOption.value);
      setGroups(data.groups);
      setSchedule(data.weekends);

      setIsLoading(false);
    },
    [semesters, schedule, readOnlyMode],
  );

  useEffect(() => {
    if (readOnlyMode && semesterID) {
      handleSemesterChange({ value: semesterID });
    }
  }, [semesterID, readOnlyMode, handleSemesterChange]);

  const toggleAvailability = (date: string, groupId: number) => {
    if (readOnlyMode) return;

    setSchedule((prev) =>
      prev.map((weekend) =>
        weekend.date === date
          ? {
              ...weekend,
              availability: {
                ...weekend.availability,
                [groupId]: !weekend.availability[groupId],
              },
            }
          : weekend,
      ),
    );
  };

  const handleSubmit = async () => {
    if (!selectedSemester || !hasTrueValues(schedule)) return;
    console.log({
      semesterId: selectedSemester.id,
      groups: groups,
      weekends: schedule,
    });
  };

  const columns = [
    {
      name: "Data",
      selector: (row: Weekend) => row.date,
    },
    ...groups.map((group) => ({
      name: group.groupName,
      cell: (row: Weekend) => (
        <input
          type="checkbox"
          checked={row.availability[group.groupId]}
          onChange={() => toggleAvailability(row.date, group.groupId)}
          disabled={readOnlyMode}
        />
      ),
    })),
  ];

  return (
    <div className="mx-auto grid max-w-5xl grid-cols-1 gap-6">
      <h1 className="text-2xl font-bold">
        {readOnlyMode
          ? "Podgląd wstępnego harmonogramu"
          : semesterID
            ? "Edytuj wstępny harmonogram"
            : "Stwórz wstępny harmonogram"}
      </h1>

      <div>
        <label htmlFor="semester" className="mb-2 block font-medium">
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
          isDisabled={readOnlyMode}
        />
      </div>

      {schedule.length > 0 && (
        <DataTable
          responsive={true}
          columns={columns}
          data={schedule}
          highlightOnHover
          progressPending={isLoading}
        />
      )}

      {!readOnlyMode && (
        <Button
          width="w-fit"
          onClick={handleSubmit}
          disabled={!hasTrueValues(schedule)}
        >
          Zapisz harmonogram
        </Button>
      )}
    </div>
  );
};

export default PreliminaryScheduleForm;
