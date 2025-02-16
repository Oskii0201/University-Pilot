import React from "react";
import PreliminaryScheduleForm from "@/components/schedule-builder/PreliminaryScheduleForm";

export default async function PreliminaryScheduleDetails({
  params,
}: {
  params: Promise<{ id: string }>;
}) {
  const { id } = await params;

  return (
    <PreliminaryScheduleForm readOnlyMode={true} semesterID={Number(id)} />
  );
}
