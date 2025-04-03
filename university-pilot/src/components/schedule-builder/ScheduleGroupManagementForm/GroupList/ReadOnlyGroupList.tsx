"use client";

import { useState } from "react";
import {
  IoIosArrowDropdownCircle,
  IoIosArrowDroprightCircle,
} from "react-icons/io";
import { Collapse } from "react-collapse";
import { FieldOfStudyGroup, Course } from "@/app/types";
import { v4 as uuidv4 } from "uuid";

interface Props {
  groups: FieldOfStudyGroup[];
  unassignedCourses: Course[];
}

export default function ReadOnlyGroupList({ groups }: Props) {
  const [openGroupIndex, setOpenGroupIndex] = useState<number | null>(null);
  const toggleGroup = (index: number) => {
    setOpenGroupIndex((prev) => (prev === index ? null : index));
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
              <h3 className="font-semibold">{group.groupName}</h3>
              <span className="text-3xl">
                {openGroupIndex === index ? (
                  <IoIosArrowDropdownCircle />
                ) : (
                  <IoIosArrowDroprightCircle />
                )}
              </span>
            </div>

            <Collapse isOpened={openGroupIndex === index}>
              <ul className="my-2">
                {group.assignedFieldsOfStudy.map((course) => (
                  <li
                    key={uuidv4()}
                    className="ml-2 flex list-disc justify-between"
                  >
                    <span>{course}</span>
                  </li>
                ))}
              </ul>
            </Collapse>
          </li>
        ))}
      </ul>
    </div>
  );
}
