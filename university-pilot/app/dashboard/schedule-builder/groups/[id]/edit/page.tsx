import React from "react";
import ScheduleGroupManagementForm from "@/components/schedule-builder/ScheduleGroupManagementForm/ScheduleGroupManagementForm";

export default async function EditGroupSetPage({
  params,
}: {
  params: Promise<{ id: string }>;
}) {
  const id = (await params).id;

  return <ScheduleGroupManagementForm groupID={id} />;
}
