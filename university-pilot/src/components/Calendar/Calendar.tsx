"use client";

import React, { useState, useEffect } from "react";
import MonthView from "./MonthView";
import EventTableView from "./EventTableView";
import EventModal from "./EventModal";
import { calculateRange } from "@/components/Calendar/lib/calculateRange";
import { CalendarHeader } from "@/components/Calendar/CalendarHeader";
import { LoadingCircle } from "@/components/ui/LoadingCircle";
import { Event } from "@/app/types";
import { toast } from "react-toastify";

type CalendarView = "month" | "week" | "table";

interface CalendarProps {
  onDateChange?: (date: Date) => void;
  events?: Event[];
}

const Calendar: React.FC<CalendarProps> = ({ onDateChange, events = [] }) => {
  const [view, setView] = useState<CalendarView | null>(null);
  const [currentDate, setCurrentDate] = useState<Date | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [isSmallScreen, setIsSmallScreen] = useState(false);
  const [selectedEvent, setSelectedEvent] = useState<Event | null>(null);

  useEffect(() => {
    const handleResize = () => {
      const isSmall = window.matchMedia("(max-width: 768px)").matches;
      setIsSmallScreen(isSmall);

      if (isSmall && view === "month") {
        setView("table");
      }
    };

    handleResize();
    window.addEventListener("resize", handleResize);

    return () => {
      window.removeEventListener("resize", handleResize);
    };
  }, [view]);

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
  }, [view, currentDate, onDateChange]);

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

  const handleEventClick = (event: Event) => {
    setSelectedEvent(event);
  };

  const handleViewChange = (newView: CalendarView) => {
    if (isSmallScreen && newView === "month") {
      toast.warning("Widok kalendarza jest niedostępny na małych ekranach");
      return;
    }
    setView(newView);
  };

  if (isLoading || !view || !currentDate) {
    return <LoadingCircle />;
  }

  const range = calculateRange(view, currentDate);

  return (
    <div className="calendar relative">
      <CalendarHeader
        currentDate={currentDate}
        onPrevClick={() => handleNavigation("prev")}
        onNextClick={() => handleNavigation("next")}
        onTodayClick={() => handleNavigation("today")}
        onViewChange={handleViewChange}
        currentView={view}
        isSmallScreen={isSmallScreen}
      />

      {view === "month" ? (
        <MonthView
          range={range}
          currentDate={currentDate}
          events={events}
          onEventClick={handleEventClick}
        />
      ) : view === "table" ? (
        <EventTableView events={events} onEventClick={handleEventClick} />
      ) : (
        <p>Funkcja WeekView zostanie wdrożona w przyszłości.</p>
      )}

      <EventModal
        event={selectedEvent}
        onClose={() => setSelectedEvent(null)}
      />
    </div>
  );
};

export default Calendar;
