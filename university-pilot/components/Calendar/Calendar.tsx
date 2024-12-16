"use client";

import React, { useState, useEffect } from "react";
import MonthView from "./MonthView";
import WeekView from "./WeekView";
import { calculateRange } from "@/components/Calendar/lib/calculateRange";
import { CalendarHeader } from "@/components/Calendar/CalendarHeader";
import { LoadingCircle } from "@/components/LoadingCircle";
import { Event } from "@/types/Event";

type CalendarView = "month" | "week";

interface CalendarProps {
  /**
   * Callback wywoływany przy każdej zmianie daty w kalendarzu.
   * Używany do synchronizacji stanu daty w komponencie nadrzędnym.
   */
  onDateChange?: (date: Date) => void;

  /**
   * Lista eventów, które mają być wyświetlane w kalendarzu.
   * Eventy powinny być filtrowane i odpowiadać zakresowi wyświetlanemu w danym widoku.
   */
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
      onDateChange?.(currentDate); // Powiadomienie nadrzędnego komponentu

      // Obliczenie i zapis zakresu dat do localStorage
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
        <MonthView range={range} currentDate={currentDate} events={events} />
      ) : (
        <WeekView range={range} events={events} />
      )}
    </div>
  );
};

export default Calendar;
