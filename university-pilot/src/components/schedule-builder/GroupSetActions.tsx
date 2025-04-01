import Link from "next/link";
import { RiFileEditLine } from "react-icons/ri";

interface GroupSetActionsProps {
  groupId: number | string;
}

const GroupSetActions = ({ groupId }: GroupSetActionsProps) => {
  return (
    <div className="flex gap-2">
      <Link
        href={`/dashboard/schedule-builder/groups/${groupId}/edit`}
        title="Edytuj"
      >
        <RiFileEditLine className="cursor-pointer text-3xl text-mutedGold transition hover:scale-110 hover:text-yellow-500" />
      </Link>
    </div>
  );
};

export default GroupSetActions;
