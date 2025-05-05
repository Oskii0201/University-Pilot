"use client";

import React, { useState } from "react";
import { DayPilot } from "@daypilot/daypilot-lite-react";
import Calendar from "@/components/Calendar/Calendar";
import { Event, CalendarView } from "@/app/types";
import CalendarFilters, {
  FilterValues,
} from "@/components/Calendar/CalendarFilters";
import { fetchScheduleEvents } from "@/lib/api/fetchScheduleEvents";

interface CalendarViewProps {
  semesterId?: number;
}
const CalendarView: React.FC<CalendarViewProps> = ({ semesterId }) => {
  const parsedSemesterID = semesterId ? Number(semesterId) : undefined;

  const [events, setEvents] = useState<Event[] | []>([]);
  const [filters, setFilters] = useState<FilterValues | null>(null);
  const [viewType, setViewType] = useState<CalendarView>("month");
  const [currentDate, setCurrentDate] = useState(DayPilot.Date.today());

  const loadEvents = async (
    filters: FilterValues,
    date: DayPilot.Date,
    viewType: CalendarView,
  ) => {
    const body = { ...filters, currentDate: date.value, viewType };

    const { data, error } = await fetchScheduleEvents(body);

    if (!error) {
      setEvents(data);
    } else {
      console.error(error);
      setEvents([]);
    }
  };

  const handleFiltersChange = async (newFilters: FilterValues) => {
    setFilters(newFilters);
    await loadEvents(newFilters, currentDate, viewType);
  };

  const handleDateOrViewChange = async (
    newDate: DayPilot.Date,
    newView: CalendarView,
  ) => {
    setCurrentDate(newDate);
    setViewType(newView);
    if (filters) {
      await loadEvents(filters, newDate, newView);
    }
  };

  return (
    <div className="space-y-6">
      <CalendarFilters
        onSearch={handleFiltersChange}
        {...(parsedSemesterID ? { semesterID: parsedSemesterID } : {})}
      />

      <Calendar
        events={events}
        currentDate={currentDate}
        viewType={viewType}
        onDateChange={(date) => handleDateOrViewChange(date, viewType)}
        onViewTypeChange={(view) => handleDateOrViewChange(currentDate, view)}
      />
    </div>
  );
};

export default CalendarView;
