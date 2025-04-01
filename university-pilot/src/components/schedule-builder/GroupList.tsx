import Link from "next/link";
import GroupSetActions from "@/components/GroupSetActions";
import { Semester } from "@/app/types";

interface Props {
  groupSets: Semester[];
}

export default function GroupList({ groupSets }: Props) {
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
              <Link href={`/dashboard/schedule-builder/groups/${set.id}`}>
                <h2 className="text-lg font-semibold">{set.name}</h2>
              </Link>
              <p className="text-sm text-gray-600">
                Utworzono:{" "}
                {new Date(set.createDate).toLocaleDateString("pl-PL")}
              </p>
            </div>
            <GroupSetActions groupId={set.id} />
          </div>
        </li>
      ))}
    </ul>
  );
}
