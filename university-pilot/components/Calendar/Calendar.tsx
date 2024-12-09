"use client";

import React, { useState, useEffect } from "react";
import MonthView from "./MonthView";
import WeekView from "./WeekView";
import { calculateRange } from "@/components/Calendar/lib/calculateRange";
import { CalendarHeader } from "@/components/Calendar/CalendarHeader";
import { LoadingCircle } from "@/components/LoadingCircle";

type CalendarView = "month" | "week";

const Calendar: React.FC = () => {
  const [view, setView] = useState<CalendarView | null>(null);
  const [currentDate, setCurrentDate] = useState<Date | null>(null);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const savedView = localStorage.getItem(
      "calendar-view",
    ) as CalendarView | null;
    const savedDate = localStorage.getItem("calendar-date");

    setView(savedView || "month");
    setCurrentDate(savedDate ? new Date(savedDate) : new Date());
    setIsLoading(false);
  }, []);

  useEffect(() => {
    if (view) localStorage.setItem("calendar-view", view);
    if (currentDate)
      localStorage.setItem("calendar-date", currentDate.toISOString());
  }, [view, currentDate]);

  const handleNavigation = (direction: "prev" | "next" | "today") => {
    if (!currentDate || !view) return;

    const increment = view === "month" ? 1 : 7;
    const newDate = new Date(currentDate);

    if (direction === "prev") {
      if (view === "month") {
        newDate.setMonth(currentDate.getMonth() - 1);
      } else {
        newDate.setDate(currentDate.getDate() - increment);
      }
    } else if (direction === "next") {
      if (view === "month") {
        newDate.setMonth(currentDate.getMonth() + 1);
      } else {
        newDate.setDate(currentDate.getDate() + increment);
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
        <MonthView range={range} currentDate={currentDate} />
      ) : (
        <WeekView range={range} />
      )}
    </div>
  );
};

export default Calendar;
