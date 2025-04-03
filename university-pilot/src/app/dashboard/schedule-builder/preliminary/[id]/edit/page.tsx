import React from "react";
import PreliminaryScheduleForm from "@/components/schedule-builder/PreliminaryScheduleForm";

export default async function EditGroupSetPage({
  params,
}: {
  params: Promise<{ id: number }>;
}) {
  const id = (await params).id;

  return <PreliminaryScheduleForm semesterID={Number(id)} />;
}
