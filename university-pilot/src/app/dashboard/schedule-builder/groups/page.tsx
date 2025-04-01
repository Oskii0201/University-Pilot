import ScheduleBuilderNavigation from "@/components/schedule-builder/ScheduleBuilderNavigation";
import GroupList from "@/components/schedule-builder/GroupList";
import { getUpcomingSemesters } from "@/lib/api/getUpcomingSemesters";

const ScheduleBuilderGroupsPage = async () => {
  const groupSets = await getUpcomingSemesters();

  return (
    <div className="flex flex-col gap-4">
      <ScheduleBuilderNavigation />
      <GroupList groupSets={groupSets} />
    </div>
  );
};

export default ScheduleBuilderGroupsPage;
