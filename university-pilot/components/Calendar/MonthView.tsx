import React from "react";

interface MonthViewProps {
  prevMonthDates: Date[];
  currentMonthDates: Date[];
  nextMonthDates: Date[];
  rows: number;
}

const MonthView: React.FC<MonthViewProps> = ({
  prevMonthDates,
  currentMonthDates,
  nextMonthDates,
  rows,
}) => {
  const calendarDays = [
    ...prevMonthDates,
    ...currentMonthDates,
    ...nextMonthDates,
  ];

  return (
    <div className="mx-auto w-full">
      <div className="grid grid-cols-7 gap-0 bg-gray-100">
        {Array.from({ length: 7 }).map((_, i) => (
          <div key={i} className="border p-2 text-center text-sm font-semibold">
            {new Date(2024, 0, i + 1).toLocaleDateString("pl-PL", {
              weekday: "short",
            })}
          </div>
        ))}
      </div>
      <div
        className={`grid grid-cols-7 grid-rows-${rows} gap-0 text-center text-sm font-medium`}
      >
        {calendarDays.map((day, index) => {
          const isCurrentMonth =
            day.getMonth() === currentMonthDates[0].getMonth();
          return (
            <div
              key={index}
              className={`border p-2 ${
                isCurrentMonth ? "bg-offWhite" : "bg-gray-100 text-gray-400"
              }`}
            >
              {day.getDate()}
            </div>
          );
        })}
      </div>
    </div>
  );
};

export default MonthView;
