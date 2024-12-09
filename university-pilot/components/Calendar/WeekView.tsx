import React from "react";
import { format } from "date-fns";
import { pl } from "date-fns/locale";

interface DateRange {
  start: Date;
  end: Date;
}

interface WeekViewProps {
  range: DateRange;
}

const WeekView: React.FC<WeekViewProps> = ({ range }) => {
  const days: Date[] = [];
  const day = new Date(range.start);

  while (day <= range.end) {
    days.push(new Date(day));
    day.setDate(day.getDate() + 1);
  }

  return (
    <div className="grid grid-cols-7">
      {days.map((day, index) => (
        <div key={index} className="border p-2 text-center">
          <div className="font-semibold">
            {format(day, "EEEEEE", { locale: pl })}{" "}
          </div>
          <div>{format(day, "d MMM", { locale: pl })} </div>
        </div>
      ))}
    </div>
  );
};

export default WeekView;
