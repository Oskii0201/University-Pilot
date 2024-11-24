export function getWeekDays(date: Date): Date[] {
  const startOfWeek = new Date(date);
  const dayOfWeek = startOfWeek.getDay();

  const offset = dayOfWeek === 0 ? -6 : 1 - dayOfWeek;
  startOfWeek.setDate(startOfWeek.getDate() + offset);

  return Array.from({ length: 7 }, (_, i) => {
    const weekDay = new Date(startOfWeek);
    weekDay.setDate(startOfWeek.getDate() + i);
    return weekDay;
  });
}
