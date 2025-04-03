import Link from "next/link";
import GroupSetActions from "@/components/schedule-builder/GroupSetActions";
import { Semester } from "@/app/types";
import React from "react";
import AcceptAction from "@/components/schedule-builder/Preliminary/AcceptAction";

interface Props {
  groupSets: Semester[];
  basePath: string;
}

export default function SemesterList({ groupSets, basePath }: Props) {
  if (groupSets.length === 0) {
    return (
      <p className="text-gray-600">Brak zestawów grup harmonogramowych.</p>
    );
  }

  return (
    <ul className="flex flex-col gap-4">
      {groupSets.map((set) => (
        <li
          key={set.id}
          className="rounded border p-4 shadow transition hover:shadow-lg"
        >
          <div className="flex items-center justify-between">
            <div>
              <Link href={`${basePath}/${set.id}`}>
                <h2 className="text-lg font-semibold">{set.name}</h2>
              </Link>
              <p className="text-sm text-gray-600">
                Utworzono:{" "}
                {new Date(set.createDate).toLocaleDateString("pl-PL")}
              </p>
              <p className="text-sm text-gray-600">
                Data modyfikacji:{" "}
                {new Date(set.updateDate).toLocaleDateString("pl-PL")}
              </p>
            </div>
            <div className="flex flex-col items-center gap-4 md:flex-row">
              {set.creationStage && set.creationStage == 2 && (
                <div className="order-2 md:order-1">
                  <AcceptAction id={set.id} />
                </div>
              )}
              <div className="order-1 flex justify-end md:order-2">
                <GroupSetActions id={set.id} basePath={basePath} />
              </div>
            </div>
          </div>
        </li>
      ))}
    </ul>
  );
}
