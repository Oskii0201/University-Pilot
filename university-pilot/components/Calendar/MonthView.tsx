import React from "react";
import { format, isSameMonth, addDays, isSameDay } from "date-fns";
import { pl } from "date-fns/locale";
import { Event } from "@/app/types";

interface DateRange {
  start: Date;
  end: Date;
}

interface MonthViewProps {
  range: DateRange;
  currentDate: Date;
  events: Event[];
}

const MonthView: React.FC<MonthViewProps> = ({
  range,
  currentDate,
  events,
}) => {
  const days: Date[] = [];
  let day = new Date(range.start);
  while (day <= range.end) {
    days.push(new Date(day));
    day = addDays(day, 1);
  }

  const getEventsForDay = (day: Date) => {
    return events.filter((event) => isSameDay(new Date(event.startTime), day));
  };

  return (
    <div className="mx-auto w-full bg-gray-100 text-center">
      <div className="grid grid-cols-7">
        {["Pn", "Wt", "Śr", "Cz", "Pt", "Sb", "Nd"].map((day) => (
          <div key={day} className="border p-1 font-semibold">
            {day}
          </div>
        ))}

        {days.map((day, index) => {
          const dayEvents = getEventsForDay(day);

          return (
            <div
              key={index}
              className={`border p-2 ${
                isSameMonth(day, currentDate) ? "bg-offWhite" : "text-gray-500"
              }`}
            >
              <div className="font-bold">
                {format(day, "d", { locale: pl })}
              </div>

              <div className="mt-2 space-y-1">
                {dayEvents.map((event) => (
                  <div
                    key={event.id}
                    className="rounded bg-blue-200 px-1 text-sm text-blue-800"
                  >
                    {event.title}
                  </div>
                ))}
              </div>
            </div>
          );
        })}
      </div>
    </div>
  );
};

export default MonthView;
