"use client";

import { GroupSet } from "@/app/types";
import { useRouter } from "next/navigation";
import { PiFolderSimplePlusFill } from "react-icons/pi";
import GroupSetActions from "@/components/GroupSetActions";

interface Props {
  groupSets: GroupSet[];
}

export default function GroupList({ groupSets }: Props) {
  const router = useRouter();

  const handleAdd = () => router.push("/dashboard/schedule-builder/groups/new");
  const handleOpen = (id: string) =>
    router.push(`/dashboard/schedule-builder/groups/${id}`);

  return (
    <>
      <div className="flex items-center justify-between">
        <h1 className="text-2xl font-bold">Grupy dla semestrów</h1>
        <button onClick={handleAdd} title="Dodaj grupę">
          <PiFolderSimplePlusFill className="cursor-pointer text-3xl text-softGreen transition hover:scale-110 hover:text-mutedGreen" />
        </button>
      </div>

      {groupSets.length > 0 ? (
        <ul className="flex flex-col gap-4">
          {groupSets.map((set) => (
            <li
              key={set.id}
              onClick={(e) => {
                e.stopPropagation();
                handleOpen(set.id);
              }}
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
    </>
  );
}
