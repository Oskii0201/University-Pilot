"use client";

import React, { useEffect, useState } from "react";
import {
  DayPilot,
  DayPilotCalendar,
  DayPilotMonth,
} from "@daypilot/daypilot-lite-react";
import { CalendarHeader } from "@/components/Calendar/CalendarHeader";
import EventModal from "@/components/Calendar/EventModal";
import { CalendarView, Event } from "@/app/types";

interface CalendarProps {
  events: Event[];
  viewType: CalendarView;
  currentDate: DayPilot.Date;
  onDateChange: (date: DayPilot.Date) => void;
  onViewTypeChange: (view: CalendarView) => void;
}

const Calendar: React.FC<CalendarProps> = ({
  events,
  viewType,
  currentDate,
  onDateChange,
  onViewTypeChange,
}) => {
  const [selectedEvent, setSelectedEvent] = useState<Event | null>(null);

  const [calendarConfig, setCalendarConfig] = useState({
    startDate: currentDate,
    viewType: "Month",
    events: events,
    eventMoveHandling: "Disabled",
    eventResizeHandling: "Disabled",
    onEventClick: (args: never) => {
      setSelectedEvent(args.e.data);
    },
    locale: "pl-pl",
  });
  useEffect(() => {
    setCalendarConfig((prevConfig) => ({
      ...prevConfig,
      events: events,
      viewType: viewType === "month" ? "Month" : "Week",
      startDate: currentDate,
    }));
  }, [events, viewType, currentDate]);
  const changeDate = (direction: number) => {
    const newDate = currentDate
      .addMonths(viewType === "month" ? direction : 0)
      .addDays(viewType === "week" ? direction * 7 : 0);
    onDateChange(newDate);
  };

  return (
    <div>
      <CalendarHeader
        currentDate={currentDate.toDate()}
        onPrevClick={() => changeDate(-1)}
        onNextClick={() => changeDate(1)}
        onTodayClick={() => onDateChange(DayPilot.Date.today())}
        onViewChange={() =>
          onViewTypeChange(viewType === "month" ? "week" : "month")
        }
        currentView={viewType}
      />

      {viewType === "month" ? (
        <DayPilotMonth {...calendarConfig} />
      ) : (
        <DayPilotCalendar {...calendarConfig} />
      )}

      <EventModal
        event={selectedEvent}
        onClose={() => setSelectedEvent(null)}
      />
    </div>
  );
};

export default Calendar;
