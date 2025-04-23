"use client";

import React, { useEffect } from "react";
import Select from "react-select";
import { useSemestersByStatus } from "@/hooks/schedule-builder/useSemestersByStatus";

export interface FilterValues {
  semesterId: number;
  fieldOfStudyId: number;
  selectedSemesterNumber: number;
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
  } = useSemestersByStatus(6, parsedSemesterID);

  useEffect(() => {
    if (
      selectedSemester?.id &&
      selectedFieldOfStudy?.id &&
      selectedSemesterNumber
    ) {
      onSearch({
        semesterId: selectedSemester.id,
        fieldOfStudyId: selectedFieldOfStudy.id,
        selectedSemesterNumber: selectedSemesterNumber,
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
                  value: selectedFieldOfStudy.id,
                  label: selectedFieldOfStudy.name,
                }
              : null
          }
          options={fieldsOfStudy.map((f) => ({ value: f.id, label: f.name }))}
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
          options={[1, 2, 3, 4, 5, 6, 7].map((n) => ({
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
