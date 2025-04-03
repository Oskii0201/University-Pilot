import React from "react";
import PreliminaryScheduleForm from "@/components/schedule-builder/PreliminaryScheduleForm";
import GroupSetActions from "@/components/schedule-builder/GroupSetActions";

export default async function PreliminaryScheduleDetails({
  params,
}: {
  params: Promise<{ id: string }>;
}) {
  const { id } = await params;

  return (
    <div className="p-4">
      <div className="flex flex-row justify-end pb-2">
        <GroupSetActions
          id={id}
          basePath="/dashboard/schedule-builder/preliminary"
        />
      </div>

      <PreliminaryScheduleForm readOnlyMode={true} semesterID={Number(id)} />
    </div>
  );
}
