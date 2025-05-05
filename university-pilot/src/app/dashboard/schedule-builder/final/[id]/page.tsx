import React from "react";
import CalendarView from "@/components/Calendar/CalendarView";

export default async function FinalScheduleDetails({
  params,
}: {
  params: Promise<{ id: string }>;
}) {
  const { id } = await params;

  return <CalendarView semesterId={Number(id)} />;
}
