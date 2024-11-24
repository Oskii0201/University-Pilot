"use client";
import React, { useState } from "react";
import { CalendarHeader } from "./CalendarHeader";

export const Calendar: React.FC = () => {
  const [currentDate, setCurrentDate] = useState(new Date());
  const [view, setView] = useState<"month" | "week">("month");
  const handlePrevClick = () => {
    const newDate = new Date(currentDate);
    if (view === "month") {
      newDate.setMonth(currentDate.getMonth() - 1);
    } else {
      newDate.setDate(currentDate.getDate() - 7);
    }
    setCurrentDate(newDate);
  };

  const handleNextClick = () => {
    const newDate = new Date(currentDate);
    if (view === "month") {
      newDate.setMonth(currentDate.getMonth() + 1);
    } else {
      newDate.setDate(currentDate.getDate() + 7);
    }
    setCurrentDate(newDate);
  };

  const handleTodayClick = () => {
    const newDate = new Date();
    setCurrentDate(newDate);
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
      {view === "month" ? <p className="">Month</p> : <p className="">Week</p>}
    </div>
  );
};
