"use client";

import React, { useEffect, useState } from "react";
import Calendar from "@/components/Calendar/Calendar";
import { fetchEvents } from "@/lib/fetchEvents";
import { Event } from "@/app/types";

export default function CalendarPage() {
  const [calendarDate, setCalendarDate] = useState<Date | null>(null);
  const [events, setEvents] = useState<Event[]>([]);

  useEffect(() => {
    if (!calendarDate) return;

    const savedRange = sessionStorage.getItem("calendar-range");
    if (savedRange) {
      const { start, end } = JSON.parse(savedRange);

      const loadEvents = async () => {
        const fetchedEvents = await fetchEvents(new Date(start), new Date(end));
        setEvents(fetchedEvents);
      };

      loadEvents();
    }
  }, [calendarDate]);

  return (
    <Calendar
      onDateChange={setCalendarDate} // Przekazanie callbacku do aktualizacji daty
      events={events} // Przekazanie eventów do kalendarza
    />
  );
}
