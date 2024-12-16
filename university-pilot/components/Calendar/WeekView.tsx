import React from "react";
import { format, isSameDay, differenceInMinutes, addMinutes } from "date-fns";
import { pl } from "date-fns/locale";
/**
 * Komponent WeekView - Przyszła funkcjonalność systemu
 * ---------------------------------------------------
 * WeekView miał za zadanie wyświetlać wydarzenia w widoku tygodniowym,
 * z siatką godzinową podobną do Google Calendar.
 *
 * Powód wyłączenia:
 * - Implementacja wymaga dodatkowej optymalizacji.
 * - Należy dopracować responsywność oraz dokładne pozycjonowanie eventów.
 * - Wymaga dużej ilości testów na różnych urządzeniach i przeglądarkach.
 *
 * Aktualne rozwiązanie (MVP):
 * - Stworzymy tabelę wyświetlającą tylko dni z eventami dla konkretnego miesiąca.
 * - Widok będzie zoptymalizowany pod kątem małych ekranów.
 *
 * Status: Komponent przeniesiony na osobny branch/funkcjonalność.
 */

interface Event {
  id: string;
  title: string;
  description: string;
  startTime: string;
  endTime: string;
}

interface DateRange {
  start: Date;
  end: Date;
}

interface WeekViewProps {
  range: DateRange;
  events: Event[];
}

const WeekView: React.FC<WeekViewProps> = ({ range, events }) => {
  const hours = Array.from({ length: 24 }, (_, i) =>
    addMinutes(range.start, i * 60),
  );

  const getEventsForDay = (day: Date) => {
    return events.filter((event) => isSameDay(new Date(event.startTime), day));
  };

  const calculateHeight = (startTime: string, endTime: string) => {
    const duration = differenceInMinutes(
      new Date(endTime),
      new Date(startTime),
    );
    if (duration < 45) return 0.5;
    return Math.ceil(duration / 60);
  };

  return (
    <div className="grid grid-cols-8">
      <div className="col-span-1 border-r border-gray-300 bg-gray-200 p-2 text-center font-semibold">
        Godziny
      </div>
      {Array.from({ length: 7 }, (_, i) =>
        addMinutes(range.start, i * 1440),
      ).map((day, index) => (
        <div
          key={index}
          className="col-span-1 border-r border-gray-300 bg-gray-100 p-2 text-center font-semibold"
        >
          {format(day, "EEEEEE", { locale: pl })}{" "}
          {format(day, "d MMM", { locale: pl })}
        </div>
      ))}

      {hours.map((hour, rowIndex) => (
        <React.Fragment key={rowIndex}>
          <div className="col-span-1 border-b border-r border-gray-300 p-2 text-center text-sm">
            {format(hour, "HH:mm")}
          </div>

          {Array.from({ length: 7 }, (_, colIndex) => {
            const day = addMinutes(range.start, colIndex * 1440);
            const dayEvents = getEventsForDay(day);
            const currentHour = hour;

            const event = dayEvents.find(
              (event) =>
                new Date(event.startTime).getHours() ===
                  currentHour.getHours() &&
                new Date(event.startTime).getMinutes() ===
                  currentHour.getMinutes(),
            );

            if (event) {
              const height =
                calculateHeight(event.startTime, event.endTime) * 50; // 50px = 1h
              return (
                <div
                  key={`${rowIndex}-${colIndex}`}
                  className="relative col-span-1 border-b border-r border-gray-300"
                  style={{ height: `${height}px` }}
                >
                  <div className="absolute left-0 right-0 top-0 rounded bg-blue-300 p-1 text-sm">
                    {event.title}
                  </div>
                </div>
              );
            }

            return (
              <div
                key={`${rowIndex}-${colIndex}`}
                className="col-span-1 border-b border-r border-gray-300"
              />
            );
          })}
        </React.Fragment>
      ))}
    </div>
  );
};

export default WeekView;
