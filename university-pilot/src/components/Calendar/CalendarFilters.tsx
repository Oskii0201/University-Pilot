"use client";

import React, { useEffect, useState } from "react";
import Select from "react-select";
import { useSemestersByStatus } from "@/hooks/schedule-builder/useSemestersByStatus";
import { getSemesterStatus } from "@/lib/api/schedule-builder/getSemesterStatus";

export interface FilterValues {
  semesterId: number;
  name: string;
  semester: number;
}

interface CalendarFiltersProps {
  onSearch: (filters: FilterValues) => void;
  semesterID?: number;
}

const CalendarFilters: React.FC<CalendarFiltersProps> = ({
  semesterID,
  onSearch,
}) => {
  const parsedSemesterID = semesterID ? Number(semesterID) : undefined;
  const [status, setStatus] = useState<number | null>(null);

  useEffect(() => {
    const resolveStatus = async () => {
      try {
        if (!parsedSemesterID) {
          return;
        }
        const response = await getSemesterStatus(parsedSemesterID);
        setStatus(response.data);
      } catch (e) {
        console.error("Błąd w checkSemesterStatus:", e);
      }
    };

    if (parsedSemesterID) {
      resolveStatus();
    }
  }, [parsedSemesterID]);

  const {
    semesters,
    selectedSemester,
    fieldsOfStudy,
    selectedFieldOfStudy,
    selectedSemesterNumber,
    isLoading,
    handleSemesterChange,
    handleFieldOfStudyChange,
    handleSemesterNumberChange,
  } = useSemestersByStatus(status || 6, parsedSemesterID);

  useEffect(() => {
    if (
      selectedSemester?.id &&
      selectedFieldOfStudy?.name &&
      selectedSemesterNumber
    ) {
      onSearch({
        semesterId: selectedSemester.id,
        name: selectedFieldOfStudy.name,
        semester: selectedSemesterNumber,
      });
    }
  }, [selectedSemester, selectedFieldOfStudy, selectedSemesterNumber]);

  return (
    <div className="grid grid-cols-1 gap-6 md:grid-cols-3">
      <div className="flex flex-col gap-4">
        <label htmlFor="semester" className="mb-2 block font-medium">
          Semestr:
        </label>
        <Select
          instanceId="semester-select"
          value={
            selectedSemester
              ? { value: selectedSemester.id, label: selectedSemester.name }
              : null
          }
          options={semesters.map((s) => ({ value: s.id, label: s.name }))}
          onChange={(e) =>
            handleSemesterChange({ value: e.value, label: e.label })
          }
          isSearchable
          placeholder="Wybierz semestr..."
          isLoading={isLoading}
          isDisabled={!!semesterID}
        />
      </div>

      <div className="flex flex-col gap-4">
        <label htmlFor="fieldOfStudy" className="mb-2 block font-medium">
          Kierunek:
        </label>
        <Select
          instanceId="fieldOfStudy-select"
          value={
            selectedFieldOfStudy
              ? {
                  value: selectedFieldOfStudy.name,
                  label: selectedFieldOfStudy.name,
                }
              : null
          }
          options={fieldsOfStudy.map((f) => ({ value: f.name, label: f.name }))}
          onChange={(e) =>
            handleFieldOfStudyChange({ value: e.value, label: e.label })
          }
          isSearchable
          placeholder="Wybierz kierunek..."
          isLoading={isLoading}
          isDisabled={!selectedSemester}
        />
      </div>

      <div className="flex flex-col gap-4">
        <label htmlFor="semesterNumber" className="mb-2 block font-medium">
          Numer semestru:
        </label>
        <Select
          instanceId="semesterNumber-select"
          value={
            selectedSemesterNumber
              ? { value: selectedSemesterNumber, label: selectedSemesterNumber }
              : null
          }
          options={selectedFieldOfStudy?.semesters.map((n) => ({
            value: n,
            label: n,
          }))}
          onChange={(e) => handleSemesterNumberChange({ value: e.value })}
          placeholder="Wybierz numer semestru..."
          isLoading={isLoading}
          isDisabled={!selectedFieldOfStudy || !selectedSemester}
        />
      </div>
    </div>
  );
};

export default CalendarFilters;
