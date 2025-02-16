"use client";

import React from "react";
import { useRouter } from "next/navigation";
import axios from "axios";
import { RiFileEditLine } from "react-icons/ri";
import { CiSquareRemove } from "react-icons/ci";

interface GroupSetActionsProps {
  groupId: string;
}

const GroupSetActions: React.FC<GroupSetActionsProps> = ({ groupId }) => {
  const router = useRouter();

  const handleDeleteGroupSet = async (id: string) => {
    const confirmDelete = window.confirm(
      "Czy na pewno chcesz usunąć ten zestaw grup?",
    );
    if (!confirmDelete) return;

    try {
      await axios.delete(`/api/schedule-builder/groups?id=${id}`);
    } catch (error) {
      console.error("Błąd podczas usuwania zestawu grup:", error);
    }
  };

  const handleEditGroupSet = (id: string) => {
    router.push(`/dashboard/schedule-builder/groups/${id}/edit`);
  };

  return (
    <div className="flex gap-2">
      <button
        onClick={(e) => {
          e.stopPropagation();
          handleEditGroupSet(groupId);
        }}
        title="Edytuj"
      >
        <RiFileEditLine className="cursor-pointer text-3xl text-mutedGold transition hover:scale-110 hover:text-yellow-500" />
      </button>
      <button
        onClick={(e) => {
          e.stopPropagation();
          handleDeleteGroupSet(groupId);
        }}
        title="Usuń"
      >
        <CiSquareRemove className="cursor-pointer text-3xl text-softRed transition hover:scale-110 hover:text-mutedRed" />
      </button>
    </div>
  );
};

export default GroupSetActions;
