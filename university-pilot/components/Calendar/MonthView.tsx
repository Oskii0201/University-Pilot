import React from "react";
import { format, isSameMonth, addDays } from "date-fns";
import { pl } from "date-fns/locale";

interface DateRange {
  start: Date;
  end: Date;
}

interface MonthViewProps {
  range: DateRange;
  currentDate: Date;
}

const MonthView: React.FC<MonthViewProps> = ({ range, currentDate }) => {
  const days: Date[] = [];
  let day = new Date(range.start);

  while (day <= range.end) {
    days.push(new Date(day));
    day = addDays(day, 1);
  }

  return (
    <div className="mx-auto w-full bg-gray-100 text-center">
      <div className="grid grid-cols-7">
        {["Pn", "Wt", "Śr", "Cz", "Pt", "Sb", "Nd"].map((day) => (
          <div key={day} className="border p-1 font-semibold">
            {day}
          </div>
        ))}

        {days.map((day, index) => (
          <div
            key={index}
            className={`border p-2 ${
              isSameMonth(day, currentDate) ? "bg-offWhite" : "text-gray-500"
            }`}
          >
            {format(day, "d", { locale: pl })}{" "}
          </div>
        ))}
      </div>
    </div>
  );
};

export default MonthView;
