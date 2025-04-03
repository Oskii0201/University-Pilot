import apiClient from "@/lib/apiClient";
import { handleApiError } from "@/utils/handleApiError";

export interface FileTypesResponse {
  [key: string]: string;
}

/**
 * Pobiera dostępne typy plików, które mogą być przesłane
 * @returns Promise z obiektem mapującym typy plików
 */
export const getFileTypes = async (): Promise<{
  data: FileTypesResponse | null;
  error: string | null;
}> => {
  try {
    const response = await apiClient.get("/File/GetFileTypes");
    return { data: response.data, error: null };
  } catch (error) {
    console.error("Błąd podczas pobierania typów plików:", error);
    return {
      data: null,
      error: handleApiError(error),
    };
  }
};
