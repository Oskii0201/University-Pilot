"use client";

import React, { useEffect, useState } from "react";
import {
  DayPilot,
  DayPilotCalendar,
  DayPilotMonth,
} from "@daypilot/daypilot-lite-react";
import { CalendarHeader } from "@/components/Calendar/CalendarHeader";
import EventModal from "@/components/Calendar/EventModal";
import { Event } from "@/app/types";

type CalendarView = "month" | "week";
interface CalendarProps {
  events: Event[];
}

const Calendar: React.FC<CalendarProps> = ({ events }) => {
  const [viewType, setViewType] = useState<CalendarView>("month");
  const [currentDate, setCurrentDate] = useState(DayPilot.Date.today());
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
    setCurrentDate(
      currentDate
        .addMonths(viewType === "month" ? direction : 0)
        .addDays(viewType === "week" ? direction * 7 : 0),
    );
  };

  return (
    <div>
      <CalendarHeader
        currentDate={currentDate.toDate()}
        onPrevClick={() => changeDate(-1)}
        onNextClick={() => changeDate(1)}
        onTodayClick={() => setCurrentDate(DayPilot.Date.today())}
        onViewChange={() =>
          setViewType((prev) => (prev === "month" ? "week" : "month"))
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
