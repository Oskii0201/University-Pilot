import React from "react";
import { MdKeyboardArrowLeft, MdKeyboardArrowRight } from "react-icons/md";

interface CalendarHeaderProps {
  currentDate: Date;
  onPrevClick: () => void;
  onNextClick: () => void;
  onTodayClick: () => void;
  onViewChange: (view: "month" | "week") => void;
  currentView: "month" | "week";
}

export const CalendarHeader: React.FC<CalendarHeaderProps> = ({
  currentDate,
  onPrevClick,
  onNextClick,
  onTodayClick,
  onViewChange,
  currentView,
}) => {
  return (
    <div className="flex items-center justify-between border-b p-4">
      <div className="flex items-center gap-4">
        <button
          onClick={onTodayClick}
          className="rounded bg-softBlue px-4 py-2 text-white transition-colors hover:bg-darkBlue"
        >
          Dziś
        </button>
        <div className="flex items-center gap-1">
          <button
            onClick={onPrevClick}
            className="rounded-full p-1 text-xl transition-colors hover:bg-gray-300"
            title="Poprzedni miesiąc"
          >
            <MdKeyboardArrowLeft />
          </button>
          <button
            onClick={onNextClick}
            className="rounded-full p-1 text-xl transition-colors hover:bg-gray-300"
            title="Następny miesiąc"
          >
            <MdKeyboardArrowRight />
          </button>
        </div>
        <h2 className="text-xl font-semibold">
          {currentDate.toLocaleDateString("pl-PL", {
            month: "long",
            year: "numeric",
          })}
        </h2>
      </div>
      <div className="flex items-center">
        <button
          onClick={() => onViewChange("month")}
          className={`rounded-l px-4 py-2 ${
            currentView === "month" ? "bg-blue-500 text-white" : "bg-gray-300"
          }`}
        >
          Miesiąc
        </button>
        <button
          onClick={() => onViewChange("week")}
          className={`rounded-r px-4 py-2 ${
            currentView === "week" ? "bg-softBlue text-white" : "bg-gray-300"
          }`}
        >
          Tydzień
        </button>
      </div>
    </div>
  );
};
