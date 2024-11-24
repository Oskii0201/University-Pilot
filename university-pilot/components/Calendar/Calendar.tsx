"use client";
import React, { useState, useEffect } from "react";
import { getCalendarDays } from "@/components/Calendar/lib/getCalendarDays";
import { getWeekDays } from "@/components/Calendar/lib/getWeekDays";
import MonthView from "@/components/Calendar/MonthView";
import { CalendarHeader } from "./CalendarHeader";

interface MonthData {
  type: "month";
  prevMonthDates: Date[];
  currentMonthDates: Date[];
  nextMonthDates: Date[];
  rows: number;
}

interface WeekData {
  type: "week";
  weekDays: Date[];
}

type CalendarData = MonthData | WeekData;

export const Calendar: React.FC = () => {
  const [currentDate, setCurrentDate] = useState(new Date());
  const [calendarData, setCalendarData] = useState<CalendarData | null>(null);
  const [view, setView] = useState<"month" | "week">("month");

  useEffect(() => {
    const fetchCalendarData = async () => {
      if (view === "month") {
        const calendarDays = getCalendarDays(currentDate);
        setCalendarData({
          type: "month",
          ...calendarDays,
        });
      } else if (view === "week") {
        const weekDays = getWeekDays(currentDate);
        setCalendarData({
          type: "week",
          weekDays,
        });
      }
    };
    fetchCalendarData().catch(console.error);
  }, [currentDate, view]);

  const handlePrevClick = () => {
    const newDate = new Date(currentDate);
    if (view === "month") {
      newDate.setMonth(currentDate.getMonth() - 1);
    } else if (view === "week") {
      newDate.setDate(currentDate.getDate() - 7);
    }
    setCurrentDate(newDate);
  };

  const handleNextClick = () => {
    const newDate = new Date(currentDate);
    if (view === "month") {
      newDate.setMonth(currentDate.getMonth() + 1);
    } else if (view === "week") {
      newDate.setDate(currentDate.getDate() + 7);
    }
    setCurrentDate(newDate);
  };

  const handleTodayClick = () => {
    setCurrentDate(new Date());
  };

  return (
    <div className="calendar">
      <CalendarHeader
        currentDate={currentDate}
        onPrevClick={handlePrevClick}
        onNextClick={handleNextClick}
        onTodayClick={handleTodayClick}
        onViewChange={setView}
        currentView={view}
      />

      {view === "month" && calendarData?.type === "month" && (
        <MonthView
          prevMonthDates={calendarData.prevMonthDates}
          currentMonthDates={calendarData.currentMonthDates}
          nextMonthDates={calendarData.nextMonthDates}
          rows={calendarData.rows}
        />
      )}

      {view === "week" && calendarData?.type === "week" && <p>Week</p>}
    </div>
  );
};
