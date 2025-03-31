import React from "react";
import { getFieldsOfStudyAssignmentsToGroup } from "@/app/lib/api/getFieldsOfStudyAssignmentsToGroup";
import UnassignedCourses from "@/components/schedule-builder/ScheduleGroupManagementForm/UnassignedCoursesList";
import GroupList from "@/components/schedule-builder/ScheduleGroupManagementForm/GroupList";
import GroupSetActions from "@/components/GroupSetActions";

export default async function GroupSetDetails({
  params,
}: {
  params: Promise<{ id: string }>;
}) {
  const { id } = await params;

  const { groups, unassignedCourses } =
    await getFieldsOfStudyAssignmentsToGroup(Number(id));

  return (
    <div className="p-4">
      <div className="flex flex-row justify-end pb-2">
        <GroupSetActions groupId={id} />
      </div>

      <h1 className="text-2xl font-bold">Zestaw grup - ID: {id}</h1>
      <p className="text-gray-600">Liczba grup: {groups.length}</p>
      <div className="flex flex-col gap-4">
        {unassignedCourses.length > 0 && (
          <UnassignedCourses unassignedCourses={unassignedCourses} />
        )}

        <GroupList groups={groups} unassignedCourses={[]} readOnlyMode={true} />
      </div>
    </div>
  );
}
