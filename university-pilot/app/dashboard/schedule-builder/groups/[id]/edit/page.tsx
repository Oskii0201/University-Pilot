import React from "react";
import ScheduleGroupManagementForm from "@/components/schedule-builder/ScheduleGroupManagementForm/ScheduleGroupManagementForm";

export default async function EditGroupSetPage({
  params,
}: {
  params: Promise<{ id: number }>;
}) {
  const id = (await params).id;

  return <ScheduleGroupManagementForm semesterID={id} />;
}
