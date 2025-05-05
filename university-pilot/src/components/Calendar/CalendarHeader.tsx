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

export const CalendarHeader: React.FC<CalendarHeaderProps> = ({ ...props }) => {
  return (
    <div className="flex flex-col justify-between gap-4 border-b p-4 md:flex-row">
      <div className="flex flex-wrap items-center gap-4 md:gap-6">
        <button
          onClick={props.onTodayClick}
          className="rounded bg-softBlue px-4 py-2 text-sm text-white transition-colors hover:bg-darkBlue md:text-base"
        >
          Dziś
        </button>
        <div className="flex items-center gap-1">
          <button
            onClick={props.onPrevClick}
            className="rounded-full p-2 text-lg transition-colors hover:bg-gray-300 md:text-xl"
            title="Poprzedni miesiąc"
          >
            <MdKeyboardArrowLeft />
          </button>
          <button
            onClick={props.onNextClick}
            className="rounded-full p-2 text-lg transition-colors hover:bg-gray-300 md:text-xl"
            title="Następny miesiąc"
          >
            <MdKeyboardArrowRight />
          </button>
        </div>
        <h2 className="text-center text-lg font-semibold md:text-left md:text-xl">
          {props.currentDate.toLocaleDateString("pl-PL", {
            month: "long",
            year: "numeric",
          })}
        </h2>
      </div>

      <div className="flex items-center">
        <button
          onClick={() => props.onViewChange("month")}
          className={`rounded-l px-3 py-2 text-sm md:px-4 md:text-base ${
            props.currentView === "month"
              ? "bg-blue-500 text-white"
              : "bg-gray-300"
          }`}
        >
          Miesiąc
        </button>
        <button
          onClick={() => props.onViewChange("week")}
          className={`rounded-r px-3 py-2 text-sm md:px-4 md:text-base ${
            props.currentView === "week"
              ? "bg-softBlue text-white"
              : "bg-gray-300"
          }`}
        >
          Tydzień
        </button>
      </div>
    </div>
  );
};
