function getDaysInMonth(year: number, month: number): number {
  return new Date(year, month + 1, 0).getDate();
}

function calculateStartOffset(firstDay: number): number {
  return firstDay === 0 ? 6 : firstDay - 1;
}

function calculateEndOffset(lastDay: number): number {
  return lastDay === 0 ? 0 : 7 - lastDay;
}

export function getCalendarDays(date: Date) {
  const year = date.getFullYear();
  const month = date.getMonth();

  const daysInMonth = getDaysInMonth(year, month);
  const firstDayOfMonth = new Date(year, month, 1).getDay();
  const lastDayOfMonth = new Date(year, month, daysInMonth).getDay();

  const startOffset = calculateStartOffset(firstDayOfMonth);
  const endOffset = calculateEndOffset(lastDayOfMonth);

  const prevMonthDates = [];
  const currentMonthDates = [];
  const nextMonthDates = [];

  if (startOffset > 0) {
    const prevMonthLastDate = getDaysInMonth(year, month - 1);
    prevMonthDates.push(
      ...Array.from(
        { length: startOffset },
        (_, i) =>
          new Date(year, month - 1, prevMonthLastDate - startOffset + i + 1),
      ),
    );
  }

  currentMonthDates.push(
    ...Array.from(
      { length: daysInMonth },
      (_, i) => new Date(year, month, i + 1),
    ),
  );

  const remainingDays =
    7 * Math.ceil((daysInMonth + startOffset + endOffset) / 7) -
    (startOffset + daysInMonth);
  nextMonthDates.push(
    ...Array.from(
      { length: remainingDays },
      (_, i) => new Date(year, month + 1, i + 1),
    ),
  );

  return {
    prevMonthDates,
    currentMonthDates,
    nextMonthDates,
    rows: Math.ceil((daysInMonth + startOffset + endOffset) / 7),
  };
}
