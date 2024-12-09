import { startOfMonth, endOfMonth, startOfWeek, endOfWeek } from "date-fns";

/**
 * Oblicza zakres dat dla widoku miesięcznego lub tygodniowego,
 * uwzględniając tygodnie od poniedziałku do niedzieli.
 */
export const calculateRange = (view: "month" | "week", date: Date) => {
  if (view === "month") {
    const start = startOfWeek(startOfMonth(date), { weekStartsOn: 1 }); // Poniedziałek
    const end = endOfWeek(endOfMonth(date), { weekStartsOn: 1 }); // Niedziela
    return { start, end };
  } else {
    const start = startOfWeek(date, { weekStartsOn: 1 });
    const end = endOfWeek(date, { weekStartsOn: 1 });
    return { start, end };
  }
};
