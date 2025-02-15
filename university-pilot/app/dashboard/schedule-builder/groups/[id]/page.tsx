import React from "react";
import axios from "axios";
import { Group } from "@/app/types";

export default async function GroupSetDetails({
  params,
}: {
  params: Promise<{ id: string }>;
}) {
  const id = (await params).id;

  const fetchGroupSet = async (id: string) => {
    try {
      const baseUrl =
        process.env.NEXT_PUBLIC_BASE_URL || "http://localhost:3000";
      const url = `${baseUrl}/api/schedule-builder/groups?id=${id}`;
      const response = await axios.get(url);
      return response.data;
    } catch (error) {
      console.error("Błąd podczas pobierania danych zestawu grup:", error);
      return null;
    }
  };

  const groupSet = await fetchGroupSet(id);

  if (!groupSet) {
    console.log("Group set not found for ID:", id);
    return <p>Nie znaleziono zestawu grup.</p>;
  }

  return (
    <div>
      <h1 className="text-2xl font-bold">{groupSet.name}</h1>
      <p className="text-gray-600">
        Utworzono: {new Date(groupSet.createdAt).toLocaleDateString()}
      </p>
      <ul className="mt-4">
        {groupSet.groups.map((group: Group) => (
          <li key={group.id} className="mb-2">
            <h2 className="font-semibold">{group.name}</h2>
            <p className="text-sm text-gray-600">
              Kursy: {group.courses.join(", ")}
            </p>
          </li>
        ))}
      </ul>
    </div>
  );
}
