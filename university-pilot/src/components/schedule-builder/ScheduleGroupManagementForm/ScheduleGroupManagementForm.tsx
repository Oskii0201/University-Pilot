"use client";

import React from "react";
import Select from "react-select";
import { Button } from "@/components/ui/Button";
import GroupList from "@/components/schedule-builder/ScheduleGroupManagementForm/GroupList/GroupList";
import UnassignedCourses from "@/components/schedule-builder/ScheduleGroupManagementForm/UnassignedCoursesList";
import { useSemesters } from "@/hooks/schedule-builder/useSemesters";
import { useGroupAssignments } from "@/hooks/schedule-builder/ScheduleGroupManagementForm/useGroupAssignment";
import { useUnsavedChanges } from "@/hooks/schedule-builder/useUnsavedChanges";

interface ScheduleGroupManagementFormProps {
  semesterID?: number;
}

const ScheduleGroupManagementForm: React.FC<
  ScheduleGroupManagementFormProps
> = ({ semesterID }) => {
  const parsedSemesterID = semesterID ? Number(semesterID) : undefined;

  const {
    semesters,
    selectedSemester,
    isLoading: semestersLoading,
    handleSemesterChange,
  } = useSemesters(0, parsedSemesterID);

  const {
    groups,
    unassignedCourses,
    isLoading: groupsLoading,
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
  } = useGroupAssignments(parsedSemesterID);

  useUnsavedChanges(hasUnsavedChanges);

  const onSemesterChange = async (selectedOption: { value: number } | null) => {
    if (!selectedOption) return;

    const semester = await handleSemesterChange(
      selectedOption,
      hasUnsavedChanges,
    );
    if (semester) {
      await fetchGroupAssignments(selectedOption.value);
    }
  };

  const isLoading = semestersLoading || groupsLoading;

  const totalAssignedCourses = getTotalAssignedCourses();
  const totalCourses = getTotalCourses();

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
          onChange={onSemesterChange}
          isSearchable
          placeholder="Wybierz semestr..."
          isLoading={isLoading}
          isDisabled={!!semesterID}
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

              <Button
                onClick={() =>
                  selectedSemester && handleSubmit(selectedSemester.id)
                }
                color="green"
                disabled={isLoading}
              >
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
