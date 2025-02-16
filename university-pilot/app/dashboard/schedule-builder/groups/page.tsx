"use client";

import React, { useEffect, useState } from "react";
import ScheduleBuilderNavigation from "@/components/schedule-builder/ScheduleBuilderNavigation";
import axios from "axios";
import { useRouter } from "next/navigation";
import { PiFolderSimplePlusFill } from "react-icons/pi";
import { GroupSet } from "@/app/types";
import { LoadingCircle } from "@/components/LoadingCircle";
import GroupSetActions from "@/components/GroupSetActions";

const ScheduleBuilderGroups: React.FC = () => {
  const [groupSets, setGroupSets] = useState<GroupSet[]>([]);
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const router = useRouter();

  useEffect(() => {
    const fetchGroupSets = async () => {
      setIsLoading(true);
      try {
        const response = await axios.get("/api/schedule-builder/groups");
        setGroupSets(response.data);
        setIsLoading(false);
      } catch (error) {
        console.error("Błąd podczas pobierania zestawów grup:", error);
      }
    };

    fetchGroupSets();
  }, []);

  const handleAddGroupSet = () => {
    router.push("/dashboard/schedule-builder/groups/new");
  };
  const handleDetailsGroupSet = (id: string) => {
    router.push(`/dashboard/schedule-builder/groups/${id}`);
  };

  return (
    <div className="flex flex-col gap-4">
      <ScheduleBuilderNavigation />
      <div className="flex items-center justify-between">
        <h1 className="text-2xl font-bold">Zestawy grup harmonogramowych</h1>
        <button onClick={handleAddGroupSet} title="Dodaj grupę">
          <PiFolderSimplePlusFill className="cursor-pointer text-3xl text-softGreen transition hover:scale-110 hover:text-mutedGreen" />
        </button>
      </div>
      {isLoading ? (
        <LoadingCircle isOverlay={true} />
      ) : groupSets.length > 0 ? (
        <ul className="flex flex-col gap-4">
          {groupSets.map((set) => (
            <li
              onClick={(e) => {
                e.stopPropagation();
                handleDetailsGroupSet(set.id);
              }}
              key={set.id}
              className="cursor-pointer rounded border p-4 shadow transition hover:shadow-lg"
            >
              <div className="flex items-center justify-between">
                <div>
                  <h2 className="text-lg font-semibold">{set.name}</h2>
                  <p className="text-sm text-gray-600">
                    Utworzono: {new Date(set.createdAt).toLocaleDateString()}
                  </p>
                </div>
                <GroupSetActions groupId={set.id} />
              </div>
            </li>
          ))}
        </ul>
      ) : (
        <p className="text-gray-600">Brak zestawów grup harmonogramowych.</p>
      )}
    </div>
  );
};

export default ScheduleBuilderGroups;
