import { startOfMonth, endOfMonth, startOfWeek, endOfWeek } from "date-fns";

export const calculateRange = (
  view: null | "month" | "week" | "table",
  date: Date,
) => {
  if (!view) {
    return { start: new Date(), end: new Date() };
  }

  if (view === "month") {
    const start = startOfWeek(startOfMonth(date), { weekStartsOn: 1 });
    const end = endOfWeek(endOfMonth(date), { weekStartsOn: 1 });
    return { start, end };
  } else if (view === "table") {
    const start = startOfMonth(date);
    const end = endOfMonth(date);
    return { start, end };
  } else {
    const start = startOfWeek(date, { weekStartsOn: 1 });
    const end = endOfWeek(date, { weekStartsOn: 1 });
    return { start, end };
  }
};
