"use client";

import React, { useState } from "react";
import {
  IoIosArrowDropdownCircle,
  IoIosArrowDroprightCircle,
} from "react-icons/io";
import { Collapse } from "react-collapse";
import { CiCircleRemove } from "react-icons/ci";
import Select from "react-select";
import { Group, Course, FieldOfStudyGroup } from "@/app/types";
import { v4 as uuidv4 } from "uuid";

interface GroupListProps {
  groups: Group[] | FieldOfStudyGroup[];
  unassignedCourses: Course[];
  handleEditGroupName?: (key: string, newName: string) => void;
  handleRemoveCourseFromGroup?: (key: string, courseName: string) => void;
  handleAddCourseToGroup?: (key: string, courseName: string) => void;
  handleRemoveGroup?: (key: string, courses: Course[]) => void;
  readOnlyMode?: boolean;
}

const GroupList: React.FC<GroupListProps> = ({
  groups,
  unassignedCourses,
  handleEditGroupName,
  handleRemoveCourseFromGroup,
  handleAddCourseToGroup,
  handleRemoveGroup,
  readOnlyMode = false,
}) => {
  const [openGroupIndex, setOpenGroupIndex] = useState<number | null>(null);
  const toggleGroup = (index: number) => {
    setOpenGroupIndex((prev) => (prev === index ? null : index));
  };

  const confirmRemoveGroup = (
    key: string,
    groupName: string,
    courses: Course[],
  ) => {
    if (!handleRemoveGroup) return;

    const confirmed = window.confirm(
      `Czy na pewno chcesz usunąć grupę "${groupName}"?\nWszystkie kursy zostaną przeniesione do listy nieprzypisanych kierunków.`,
    );
    if (confirmed) {
      handleRemoveGroup(key, courses);
    }
  };

  return (
    <div className="col-span-1 md:col-span-2">
      <h2 className="mb-4 text-lg font-semibold">Grupy</h2>
      <ul className="flex flex-col gap-4">
        {groups.map((group, index) => (
          <li
            key={index}
            className="cursor-pointer rounded border bg-gray-100 p-4 transition hover:shadow"
            onClick={() => toggleGroup(index)}
          >
            <div className="flex items-center justify-between">
              {!readOnlyMode ? (
                <input
                  type="text"
                  value={group.groupName}
                  onClick={(e) => e.stopPropagation()}
                  onChange={(e) =>
                    handleEditGroupName?.(group.key, e.target.value)
                  }
                  className="border-b border-dashed border-gray-400 bg-transparent font-semibold focus:outline-none"
                />
              ) : (
                <h3 className="font-semibold">{group.groupName}</h3>
              )}

              <div className="flex items-center gap-2">
                {!readOnlyMode && (
                  <button
                    onClick={(e) => {
                      e.stopPropagation();
                      confirmRemoveGroup(
                        group.key,
                        group.groupName,
                        group.assignedFieldsOfStudy,
                      );
                    }}
                    className="text-xl text-red-500 hover:underline"
                    title="Usuń grupę"
                  >
                    <CiCircleRemove />
                  </button>
                )}
                <span className="text-3xl">
                  {openGroupIndex === index ? (
                    <IoIosArrowDropdownCircle />
                  ) : (
                    <IoIosArrowDroprightCircle />
                  )}
                </span>
              </div>
            </div>

            <Collapse isOpened={openGroupIndex === index}>
              <ul className="my-2">
                {group.assignedFieldsOfStudy.map((course) => (
                  <li
                    key={uuidv4()}
                    className="ml-2 flex list-disc justify-between"
                  >
                    <span>{course}</span>

                    {!readOnlyMode && (
                      <button
                        onClick={(e) => {
                          e.stopPropagation();
                          handleRemoveCourseFromGroup?.(group.key, course);
                        }}
                        className="text-xl font-semibold text-red-500 hover:underline"
                      >
                        <CiCircleRemove />
                      </button>
                    )}
                  </li>
                ))}
              </ul>
              {!readOnlyMode && (
                <div onClick={(e) => e.stopPropagation()}>
                  <Select
                    options={unassignedCourses.map((course) => ({
                      value: course,
                      label: course,
                    }))}
                    onChange={(option) =>
                      option &&
                      handleAddCourseToGroup?.(group.key, option.value)
                    }
                    isSearchable
                    placeholder="Dodaj kierunek..."
                    value={null}
                  />
                </div>
              )}
            </Collapse>
          </li>
        ))}
      </ul>
    </div>
  );
};

export default GroupList;
