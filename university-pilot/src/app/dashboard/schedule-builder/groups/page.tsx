import ScheduleBuilderNavigation from "@/components/schedule-builder/ScheduleBuilderNavigation";
import GroupList from "@/components/schedule-builder/GroupList";
import { getUpcomingSemesters } from "@/lib/api/getUpcomingSemesters";
import Link from "next/link";
import { PiFolderSimplePlusFill } from "react-icons/pi";

const ScheduleBuilderGroupsPage = async () => {
  const { data, error } = await getUpcomingSemesters(0);

  return (
    <div className="flex flex-col gap-4">
      <ScheduleBuilderNavigation />

      <div className="flex items-center justify-between">
        <h1 className="text-2xl font-bold">Grupy dla semestrów</h1>
        <Link
          href="/dashboard/schedule-builder/groups/new"
          aria-label="Dodaj grupę"
          title="Dodaj grupę"
          className="text-3xl text-softGreen transition hover:scale-110 hover:text-mutedGreen"
        >
          <PiFolderSimplePlusFill className="cursor-pointer" />
        </Link>
      </div>

      {error ? (
        <div className="rounded bg-red-100 p-4 text-red-700 shadow">
          <p className="font-semibold">Błąd:</p>
          <p>{error}</p>
        </div>
      ) : (
        <GroupList groupSets={data ?? []} />
      )}
    </div>
  );
};

export default ScheduleBuilderGroupsPage;
