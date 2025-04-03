"use client";

import React from "react";
import Select from "react-select";
import DataTable, { TableColumn } from "react-data-table-component";
import { Button } from "@/components/ui/Button";
import { Weekend } from "@/app/types";
import { useSemesters } from "@/hooks/schedule-builder/useSemesters";
import { useUnsavedChanges } from "@/hooks/schedule-builder/useUnsavedChanges";
import { useWeekendAvailability } from "@/hooks/schedule-builder/PreliminaryScheduleForm/useWeekendAvailability";

interface PreliminaryScheduleFormProps {
  semesterID?: number;
  readOnlyMode?: boolean;
}

const PreliminaryScheduleForm: React.FC<PreliminaryScheduleFormProps> = ({
  semesterID,
  readOnlyMode,
}) => {
  const parsedSemesterID = semesterID ? Number(semesterID) : undefined;

  const {
    semesters,
    selectedSemester,
    isLoading: semestersLoading,
    handleSemesterChange,
  } = useSemesters(1, parsedSemesterID);

  const {
    schedule,
    groups,
    isLoading: weekendLoading,
    hasUnsavedChanges,
    fetchWeekendAvailability,
    toggleAvailability,
    hasTrueValues,
    handleSubmit,
  } = useWeekendAvailability(parsedSemesterID);

  useUnsavedChanges(hasUnsavedChanges);

  const onSemesterChange = async (selectedOption: { value: number } | null) => {
    if (!selectedOption) return;

    const semester = await handleSemesterChange(
      selectedOption,
      hasUnsavedChanges,
    );

    if (semester) {
      await fetchWeekendAvailability(selectedOption.value);
    }
  };

  const columns: TableColumn<Weekend>[] = [
    {
      name: "Data",
      selector: (row: Weekend) => row.date,
      cell: (row: Weekend) => {
        const dateObj = new Date(row.date);
        const formattedDate = dateObj.toLocaleDateString("pl-PL");

        const dayOfWeek = new Intl.DateTimeFormat("pl-PL", {
          weekday: "short",
        }).format(dateObj);

        return (
          <div className="w-full">
            {dayOfWeek}, {formattedDate}
          </div>
        );
      },
    },
    ...groups.map((group) => ({
      name: group.groupName,
      cell: (row: Weekend) => (
        <div className="mx-auto">
          <input
            type="checkbox"
            checked={row.availability[group.groupId]}
            onChange={() => toggleAvailability(row.date, group.groupId)}
            disabled={readOnlyMode}
            className={`peer h-4 w-4 appearance-none rounded-md border border-gray-300 checked:border-blue-500 checked:bg-blue-500 peer-disabled:border-blue-500/50 peer-disabled:bg-blue-500/50 focus:ring-2 focus:ring-blue-500 sm:h-5 sm:w-5 md:h-6 md:w-6 ${!readOnlyMode && "cursor-pointer"}`}
          />
        </div>
      ),
    })),
  ];

  const isLoading = semestersLoading || weekendLoading;

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
          onChange={onSemesterChange}
          isSearchable
          placeholder="Wybierz semestr..."
          isLoading={isLoading}
          isDisabled={readOnlyMode || !!semesterID}
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
          onClick={() => selectedSemester && handleSubmit(selectedSemester.id)}
          disabled={!hasTrueValues(schedule)}
        >
          Zapisz harmonogram
        </Button>
      )}
    </div>
  );
};

export default PreliminaryScheduleForm;
