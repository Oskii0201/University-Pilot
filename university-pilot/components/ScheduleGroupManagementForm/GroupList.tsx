import React, { useState } from "react";
import {
  IoIosArrowDropdownCircle,
  IoIosArrowDroprightCircle,
} from "react-icons/io";
import { Collapse } from "react-collapse";
import { CiCircleRemove } from "react-icons/ci";
import Select from "react-select";
import { toast } from "react-toastify";
import { Group, Course } from "@/app/types";

interface GroupListProps {
  groups: Group[];
  unassignedCourses: Course[];
  handleEditGroupName: (groupId: string, newName: string) => void;
  handleRemoveCourseFromGroup: (groupId: string, courseId: number) => void;
  handleAddCourseToGroup: (groupId: string, courseId: number) => void;
  handleRemoveGroup: (groupId: string, courses: Course[]) => void;
}

const GroupList: React.FC<GroupListProps> = ({
  groups,
  unassignedCourses,
  handleEditGroupName,
  handleRemoveCourseFromGroup,
  handleAddCourseToGroup,
  handleRemoveGroup,
}) => {
  const [openGroupId, setOpenGroupId] = useState<string | null>(null);

  const toggleGroup = (groupId: string) =>
    setOpenGroupId((prev) => (prev === groupId ? null : groupId));

  const confirmRemoveGroup = (
    groupId: string,
    groupName: string,
    courses: Course[],
  ) => {
    toast.info(
      <>
        <p>
          Czy na pewno chcesz usunąć grupę <strong>{groupName}</strong>?
        </p>
        <div className="mt-2 flex justify-end gap-2">
          <button
            onClick={() => handleRemoveGroup(groupId, courses)}
            className="rounded bg-red-500 px-4 py-2 text-white transition hover:bg-red-600"
          >
            Usuń
          </button>
        </div>
      </>,
    );
  };

  return (
    <div className="col-span-1 md:col-span-2">
      <h2 className="mb-4 text-lg font-semibold">Grupy</h2>
      <ul className="flex flex-col gap-4">
        {groups.map((group) => (
          <li
            key={group.id}
            className="cursor-pointer rounded border bg-gray-100 p-4 transition hover:shadow"
            onClick={() => toggleGroup(group.id)} // Otwieranie grupy po kliknięciu w całe li
          >
            <div className="flex items-center justify-between">
              <input
                type="text"
                value={group.name}
                onClick={(e) => e.stopPropagation()} // Zatrzymanie propagacji kliknięcia
                onChange={(e) => handleEditGroupName(group.id, e.target.value)}
                className="border-b border-dashed border-gray-400 bg-transparent font-semibold focus:outline-none"
              />
              <div className="flex items-center gap-2">
                <button
                  onClick={(e) => {
                    e.stopPropagation();
                    confirmRemoveGroup(group.id, group.name, group.courses);
                  }}
                  className="text-xl text-red-500 hover:underline"
                  title="Usuń grupę"
                >
                  <CiCircleRemove />
                </button>
                <span className="text-3xl">
                  {openGroupId === group.id ? (
                    <IoIosArrowDropdownCircle />
                  ) : (
                    <IoIosArrowDroprightCircle />
                  )}
                </span>
              </div>
            </div>
            <Collapse isOpened={openGroupId === group.id}>
              <ul className="my-2">
                {group.courses.map((course) => (
                  <li
                    key={course.id}
                    className="ml-2 flex list-disc justify-between"
                  >
                    <span>{course.name}</span>
                    <button
                      onClick={(e) => {
                        e.stopPropagation();
                        handleRemoveCourseFromGroup(group.id, course.id);
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
                    value: course.id,
                    label: course.name,
                  }))}
                  onChange={(option) =>
                    option &&
                    handleAddCourseToGroup(group.id, Number(option.value))
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
