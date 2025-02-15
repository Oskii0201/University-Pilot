import { startOfMonth, endOfMonth, startOfWeek, endOfWeek } from "date-fns";
import { calculateRange } from "@/components/Calendar/lib/calculateRange";

describe("calculateRange", () => {
  it("returns today's date for null view", () => {
    const result = calculateRange(null, new Date("2025-01-10"));
    expect(result.start.toDateString()).toEqual(new Date().toDateString());
    expect(result.end.toDateString()).toEqual(new Date().toDateString());
  });

  it("calculates the full month range for 'month' view", () => {
    const date = new Date("2025-01-10");
    const result = calculateRange("month", date);

    expect(result.start).toEqual(
      startOfWeek(startOfMonth(date), { weekStartsOn: 1 }),
    );
    expect(result.end).toEqual(
      endOfWeek(endOfMonth(date), { weekStartsOn: 1 }),
    );
  });

  it("calculates the range for 'table' view", () => {
    const date = new Date("2025-01-10");
    const result = calculateRange("table", date);

    expect(result.start).toEqual(startOfMonth(date));
    expect(result.end).toEqual(endOfMonth(date));
  });

  it("calculates the range for 'week' view", () => {
    const date = new Date("2025-01-10");
    const result = calculateRange("week", date);

    expect(result.start).toEqual(startOfWeek(date, { weekStartsOn: 1 }));
    expect(result.end).toEqual(endOfWeek(date, { weekStartsOn: 1 }));
  });
});
