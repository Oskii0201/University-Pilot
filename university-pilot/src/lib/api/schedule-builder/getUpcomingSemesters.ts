import apiClient from "@/lib/apiClient";
import { Semester } from "@/app/types";
import { handleApiError } from "@/utils/handleApiError";

/**
 * Pobiera nadchodzące semestry
 * @param count Maksymalna liczba semestrów do pobrania (domyślnie: 50)
 * @param status (opcjonalnie) status semestru
 * @returns Promise z rezultatem zapytania (dane lub błąd)
 */
export const getUpcomingSemesters = async (
  status?: number,
  count = 50,
): Promise<{ data: Semester[] | null; error: string | null }> => {
  const searchParams = new URLSearchParams({ count: count.toString() });
  if (status !== undefined) {
    searchParams.append("status", status.toString());
  }

  try {
    const response = await apiClient.get(
      `/Semester/GetUpcomingSemesters?${searchParams.toString()}`,
    );
    return { data: response.data, error: null };
  } catch (error) {
    console.error("Błąd podczas pobierania semestrów:", error);
    return { data: null, error: handleApiError(error) };
  }
};
