import React from "react";
import { Event } from "@/app/types";
import { useClickOutside } from "@/hooks/useClickOutside";

interface EventModalProps {
  event: Event | null;
  onClose: () => void;
}

const EventModal: React.FC<EventModalProps> = ({ event, onClose }) => {
  const modalRef = useClickOutside(onClose);

  if (!event) return null;

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black bg-opacity-50">
      <div
        ref={modalRef}
        className="relative w-full max-w-lg rounded-lg bg-white p-6 shadow-lg md:max-w-2xl lg:max-w-3xl"
      >
        <button
          onClick={onClose}
          className="absolute right-3 top-3 text-2xl text-gray-500 hover:text-gray-700"
        >
          ×
        </button>

        <h2 className="mb-4 text-2xl font-bold text-gray-800">{event.title}</h2>

        <div className="space-y-2 text-sm text-gray-600">
          <p>
            <span className="font-semibold">Czas:</span>{" "}
            {new Date(event.startTime).toLocaleString()} -{" "}
            {new Date(event.endTime).toLocaleString()}
          </p>
          {event.room && (
            <p>
              <span className="font-semibold">Sala:</span> {event.room}
            </p>
          )}
          {event.lecturer && (
            <p>
              <span className="font-semibold">Prowadzący:</span>{" "}
              {event.lecturer}
            </p>
          )}
          <p>
            <span className="font-semibold">Opis:</span>{" "}
            {event.description || "Brak dodatkowych szczegółów."}
          </p>
        </div>
      </div>
    </div>
  );
};

export default EventModal;
