import React from "react";
import { format } from "date-fns";
import { pl } from "date-fns/locale";
import { Event } from "@/app/types";

interface EventTableViewProps {
  events: Event[];
  onEventClick: (event: Event) => void;
}

const EventTableView: React.FC<EventTableViewProps> = ({
  events,
  onEventClick,
}) => {
  return (
    <div className="w-full">
      <div className="hidden md:block">
        <table className="w-full table-fixed border-collapse text-sm">
          <thead>
            <tr className="bg-gray-200 text-left">
              <th className="border p-2">Data</th>
              <th className="border p-2">Godzina</th>
              <th className="border p-2">Przedmiot</th>
              <th className="border p-2">Sala</th>
            </tr>
          </thead>
          <tbody>
            {events.length > 0 ? (
              events.map((event) => (
                <tr
                  key={event.id}
                  className="cursor-pointer hover:bg-gray-50"
                  onClick={() => onEventClick(event)}
                >
                  <td className="border p-2 font-semibold">
                    {format(new Date(event.startTime), "EEEE, dd.MM.yyyy", {
                      locale: pl,
                    })}
                  </td>
                  <td className="border p-2">
                    {format(new Date(event.startTime), "HH:mm")} -{" "}
                    {format(new Date(event.endTime), "HH:mm")}
                  </td>
                  <td className="border p-2">{event.title}</td>
                  <td className="border p-2">
                    {event.room || "Brak informacji"}
                  </td>
                </tr>
              ))
            ) : (
              <tr>
                <td colSpan={4} className="border p-4 text-center">
                  Brak wydarzeń w tym miesiącu.
                </td>
              </tr>
            )}
          </tbody>
        </table>
      </div>

      <div className="block space-y-2 md:hidden">
        {events.length > 0 ? (
          events.map((event) => (
            <div
              onClick={() => onEventClick(event)}
              key={event.id}
              className="cursor-pointer rounded-lg border bg-white p-2 shadow-sm"
            >
              <div className="font-semibold text-blue-600">
                {format(new Date(event.startTime), "EEEE, dd.MM.yyyy", {
                  locale: pl,
                })}{" "}
                {format(new Date(event.startTime), "HH:mm")} -{" "}
                {format(new Date(event.endTime), "HH:mm")}
              </div>
              <div className="font-bold">{event.title}</div>
              <div className="text-gray-600">
                {event.room || "Brak informacji"}
              </div>
            </div>
          ))
        ) : (
          <div className="p-4 text-center">Brak wydarzeń w tym miesiącu.</div>
        )}
      </div>
    </div>
  );
};

export default EventTableView;
