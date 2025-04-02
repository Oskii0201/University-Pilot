import { getFieldsOfStudyAssignmentsToGroup } from "@/lib/api/schedule-builder/getFieldsOfStudyAssignmentsToGroup";
import UnassignedCourses from "@/components/schedule-builder/ScheduleGroupManagementForm/UnassignedCoursesList";
import GroupList from "@/components/schedule-builder/ScheduleGroupManagementForm/GroupList/GroupList";
import GroupSetActions from "@/components/schedule-builder/GroupSetActions";
import { notFound } from "next/navigation";

export default async function GroupSetDetails({
  params,
}: {
  params: Promise<{ id: number }>;
}) {
  const id = (await params).id;
  const { data, error } = await getFieldsOfStudyAssignmentsToGroup(Number(id));

  if (error || !data) {
    return notFound();
  }

  const { assignedFieldOfStudyGroups, unassignedFieldsOfStudy } = data;

  return (
    <div className="p-4">
      <div className="flex flex-row justify-end pb-2">
        <GroupSetActions
          id={id}
          basePath="/dashboard/schedule-builder/groups"
        />
      </div>

      <h1 className="text-2xl font-bold">{data.name}</h1>
      <p className="text-gray-600">
        Liczba grup: {assignedFieldOfStudyGroups.length}
      </p>

      <div className="flex flex-col gap-4">
        {unassignedFieldsOfStudy.length > 0 && (
          <UnassignedCourses unassignedCourses={unassignedFieldsOfStudy} />
        )}

        <GroupList
          groups={assignedFieldOfStudyGroups}
          unassignedCourses={[]}
          readOnlyMode={true}
        />
      </div>
    </div>
  );
}
