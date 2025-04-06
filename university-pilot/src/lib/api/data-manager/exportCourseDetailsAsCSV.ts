import apiClient from "@/lib/apiClient";
import { handleApiError } from "@/utils/handleApiError";

/**
 * Pobiera plik CSV z danymi kursów dla danego semestru
 * @param semesterId ID semestru
 * @returns Obiekt z blobem, nazwą pliku i typem MIME lub błędem
 */
export const exportCourseDetailsAsCSV = async (
  semesterId: number,
): Promise<{
  data: Blob | null;
  fileName: string | null;
  mimeType: string | null;
  error: string | null;
}> => {
  if (!semesterId || semesterId <= 0) {
    return {
      data: null,
      fileName: null,
      mimeType: null,
      error: "Nieprawidłowe ID semestru",
    };
  }

  try {
    const response = await apiClient.get(
      `/File/export-course-details/${semesterId}`,
      {
        responseType: "blob",
      },
    );

    const mimeType = response.headers["content-type"] || "text/csv";
    const fileName = `CourseDetails_Semester_${semesterId}.csv`;

    return {
      data: response.data,
      fileName,
      mimeType,
      error: null,
    };
  } catch (error) {
    console.error("Błąd podczas pobierania pliku:", error);
    return {
      data: null,
      fileName: null,
      mimeType: null,
      error: handleApiError(error),
    };
  }
};
