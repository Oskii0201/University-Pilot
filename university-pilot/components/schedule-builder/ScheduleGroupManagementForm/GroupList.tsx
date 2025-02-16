import React, { useState } from "react";
import {
  IoIosArrowDropdownCircle,
  IoIosArrowDroprightCircle,
} from "react-icons/io";
import { Collapse } from "react-collapse";
import { CiCircleRemove } from "react-icons/ci";
import Select from "react-select";
import { Group, Course } from "@/app/types";
import { v4 as uuidv4 } from "uuid";

interface GroupListProps {
  groups: Group[];
  unassignedCourses: Course[];
  handleEditGroupName: (groupId: number, newName: string) => void;
  handleRemoveCourseFromGroup: (groupId: number, courseName: string) => void;
  handleAddCourseToGroup: (groupId: number, courseName: string) => void;
  handleRemoveGroup: (groupId: number, courses: Course[]) => void;
}

const GroupList: React.FC<GroupListProps> = ({
  groups,
  unassignedCourses,
  handleEditGroupName,
  handleRemoveCourseFromGroup,
  handleAddCourseToGroup,
  handleRemoveGroup,
}) => {
  const [openGroupId, setOpenGroupId] = useState<number | null>(null);

  const toggleGroup = (groupId: number) =>
    setOpenGroupId((prev) => (prev === groupId ? null : groupId));

  const confirmRemoveGroup = (
    groupId: number,
    groupName: string,
    courses: Course[],
  ) => {
    const confirmed = window.confirm(
      `Czy na pewno chcesz usunąć grupę "${groupName}"?\nWszystkie kursy zostaną przeniesione do listy nieprzypisanych kierunków.`,
    );
    if (confirmed) {
      handleRemoveGroup(groupId, courses);
    }
  };

  return (
    <div className="col-span-1 md:col-span-2">
      <h2 className="mb-4 text-lg font-semibold">Grupy</h2>
      <ul className="flex flex-col gap-4">
        {groups.map((group) => (
          <li
            key={group.groupId}
            className="cursor-pointer rounded border bg-gray-100 p-4 transition hover:shadow"
            onClick={() => toggleGroup(group.groupId)}
          >
            <div className="flex items-center justify-between">
              <input
                type="text"
                value={group.groupName}
                onClick={(e) => e.stopPropagation()} // Zatrzymanie propagacji kliknięcia
                onChange={(e) =>
                  handleEditGroupName(group.groupId, e.target.value)
                }
                className="border-b border-dashed border-gray-400 bg-transparent font-semibold focus:outline-none"
              />
              <div className="flex items-center gap-2">
                <button
                  onClick={(e) => {
                    e.stopPropagation();
                    confirmRemoveGroup(
                      group.groupId,
                      group.groupName,
                      group.assignedFieldsOfStudy,
                    );
                  }}
                  className="text-xl text-red-500 hover:underline"
                  title="Usuń grupę"
                >
                  <CiCircleRemove />
                </button>
                <span className="text-3xl">
                  {openGroupId === group.groupId ? (
                    <IoIosArrowDropdownCircle />
                  ) : (
                    <IoIosArrowDroprightCircle />
                  )}
                </span>
              </div>
            </div>
            <Collapse isOpened={openGroupId === group.groupId}>
              <ul className="my-2">
                {group.assignedFieldsOfStudy.map((course) => (
                  <li
                    key={uuidv4()}
                    className="ml-2 flex list-disc justify-between"
                  >
                    <span>{course}</span>
                    <button
                      onClick={(e) => {
                        e.stopPropagation();
                        handleRemoveCourseFromGroup(group.groupId, course);
                      }}
                      className="text-xl font-semibold text-red-500 hover:underline"
                    >
                      <CiCircleRemove />
                    </button>
                  </li>
                ))}
              </ul>
              <div onClick={(e) => e.stopPropagation()}>
                <Select
                  options={unassignedCourses.map((course) => ({
                    value: course,
                    label: course,
                  }))}
                  onChange={(option) =>
                    option &&
                    handleAddCourseToGroup(group.groupId, option.value)
                  }
                  isSearchable
                  placeholder="Dodaj kierunek..."
                  value={null}
                />
              </div>
            </Collapse>
          </li>
        ))}
      </ul>
    </div>
  );
};

export default GroupList;
