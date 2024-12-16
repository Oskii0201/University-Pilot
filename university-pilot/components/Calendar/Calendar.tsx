"use client";

import React, { useState, useEffect } from "react";
import MonthView from "./MonthView";
import EventTableView from "./EventTableView";
import { calculateRange } from "@/components/Calendar/lib/calculateRange";
import { CalendarHeader } from "@/components/Calendar/CalendarHeader";
import { LoadingCircle } from "@/components/LoadingCircle";
import { Event } from "@/app/types";

type CalendarView = "month" | "week" | "table";

interface CalendarProps {
  onDateChange?: (date: Date) => void;
  events?: Event[];
}

const Calendar: React.FC<CalendarProps> = ({ onDateChange, events = [] }) => {
  const [view, setView] = useState<CalendarView | null>(null);
  const [currentDate, setCurrentDate] = useState<Date | null>(null);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const savedView = sessionStorage.getItem(
      "calendar-view",
    ) as CalendarView | null;
    const savedDate = sessionStorage.getItem("calendar-date");

    setView(savedView || "month");
    setCurrentDate(savedDate ? new Date(savedDate) : new Date());
    setIsLoading(false);
  }, []);

  useEffect(() => {
    if (view) sessionStorage.setItem("calendar-view", view);
    if (currentDate) {
      sessionStorage.setItem("calendar-date", currentDate.toISOString());
      onDateChange?.(currentDate);

      const range = calculateRange(view, currentDate);
      sessionStorage.setItem(
        "calendar-range",
        JSON.stringify({
          start: range.start.toISOString(),
          end: range.end.toISOString(),
        }),
      );
    }
  }, [view, currentDate]);

  const handleNavigation = (direction: "prev" | "next" | "today") => {
    if (!currentDate || !view) return;

    const increment = view === "week" ? 7 : 1;
    const newDate = new Date(currentDate);

    if (direction === "prev") {
      if (view === "week") {
        newDate.setDate(currentDate.getDate() - increment);
      } else {
        newDate.setMonth(currentDate.getMonth() - 1);
      }
    } else if (direction === "next") {
      if (view === "week") {
        newDate.setDate(currentDate.getDate() + increment);
      } else {
        newDate.setMonth(currentDate.getMonth() + 1);
      }
    } else {
      setCurrentDate(new Date());
      return;
    }

    setCurrentDate(newDate);
  };

  if (isLoading || !view || !currentDate) {
    return <LoadingCircle />;
  }

  const range = calculateRange(view, currentDate);

  return (
    <div className="calendar">
      <CalendarHeader
        currentDate={currentDate}
        onPrevClick={() => handleNavigation("prev")}
        onNextClick={() => handleNavigation("next")}
        onTodayClick={() => handleNavigation("today")}
        onViewChange={setView}
        currentView={view}
      />

      {view === "month" ? (
        <MonthView range={range} currentDate={currentDate} events={events} />
      ) : view === "table" ? (
        <EventTableView events={events} />
      ) : (
        <p>Funkcja WeekView zostanie wdrożona w przyszłości.</p>
      )}
    </div>
  );
};

export default Calendar;
