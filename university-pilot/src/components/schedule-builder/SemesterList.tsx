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
  console.log(groupSets);
  return (
    <ul className="flex flex-col gap-4">
      {groupSets.map((set) => (
        <li
          key={set.id}
          className={`rounded border p-4 shadow transition hover:shadow-lg ${set.creationStage === 3 || (set.creationStage === 4 && "cursor-not-allowed")}`}
        >
          <div className="flex items-center justify-between">
            <div>
              {set.creationStage === 3 || set.creationStage === 4 ? (
                <h2 className="text-lg font-semibold text-gray-400">
                  {set.name}
                </h2>
              ) : (
                <Link href={`${basePath}/${set.id}`}>
                  <h2 className="text-lg font-semibold hover:underline">
                    {set.name}
                  </h2>
                </Link>
              )}
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
              {set.creationStage &&
                (set.creationStage == 2 || set.creationStage == 5) && (
                  <div className="order-2 md:order-1">
                    <AcceptAction id={set.id} stage={set.creationStage} />
                  </div>
                )}
              {set.creationStage <= 2 && (
                <div className="order-1 flex justify-end md:order-2">
                  <GroupSetActions id={set.id} basePath={basePath} />
                </div>
              )}
              {(set.creationStage == 3 || set.creationStage == 4) && (
                <div className="order-1 flex items-center justify-end gap-2 text-sm font-medium text-gray-600 md:order-2">
                  <div className="h-5 w-5 animate-spin rounded-full border-2 border-solid border-blue-500 border-t-transparent"></div>
                  <span className="hidden md:block">
                    Generowanie harmonogramu...
                  </span>
                </div>
              )}
            </div>
          </div>
        </li>
      ))}
    </ul>
  );
}
