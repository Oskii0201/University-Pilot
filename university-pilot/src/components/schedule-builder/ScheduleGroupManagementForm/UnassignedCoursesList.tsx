import React from "react";
import { Course } from "@/app/types";
import { v4 as uuidv4 } from "uuid";

interface UnassignedCoursesProps {
  unassignedCourses: Course[];
}

const UnassignedCourses: React.FC<UnassignedCoursesProps> = ({
  unassignedCourses,
}) => (
  <div className="col-span-1 md:col-span-2">
    <h2 className="mb-4 text-lg font-semibold">Nieprzypisane kierunki</h2>
    <ul className="flex max-h-64 flex-col gap-2 overflow-y-auto scrollbar-thin scrollbar-track-gray-100 scrollbar-thumb-gray-300">
      {unassignedCourses.length > 0 ? (
        unassignedCourses.map((course) => (
          <li
            key={uuidv4()}
            className="rounded border bg-gray-50 p-2 transition hover:bg-gray-100"
          >
            {course}
          </li>
        ))
      ) : (
        <li className="text-center text-gray-500">
          Brak nieprzypisanych kierunków
        </li>
      )}
    </ul>
  </div>
);

export default UnassignedCourses;
