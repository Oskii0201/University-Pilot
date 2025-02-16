import React from "react";
import { fetchGroups } from "@/app/lib/api/fetchGroups";
import { Group } from "@/app/types";

export default async function GroupSetDetails({
  params,
}: {
  params: Promise<{ id: string }>;
}) {
  const { id } = await params;

  const { groups, unassignedCourses } = await fetchGroups(Number(id));

  return (
    <div className="p-4">
      <h1 className="text-2xl font-bold">Zestaw grup - ID: {id}</h1>
      <p className="text-gray-600">Liczba grup: {groups.length}</p>

      <ul className="mt-4 space-y-4">
        {groups.map((group: Group) => (
          <li key={group.groupId} className="rounded-lg border p-4 shadow-sm">
            <h2 className="text-lg font-semibold">{group.groupName}</h2>
            <p className="text-sm text-gray-600">
              Przypisane kierunki:{" "}
              {group.assignedFieldsOfStudy.length > 0
                ? group.assignedFieldsOfStudy.join(", ")
                : "Brak"}
            </p>
          </li>
        ))}
      </ul>

      {unassignedCourses.length > 0 && (
        <div className="mt-6">
          <h2 className="text-lg font-semibold">Nieprzypisane kierunki</h2>
          <ul className="list-disc pl-4 text-gray-600">
            {unassignedCourses.map((course, index) => (
              <li key={index}>{course}</li>
            ))}
          </ul>
        </div>
      )}
    </div>
  );
}
