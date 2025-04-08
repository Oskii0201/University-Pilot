import { useCallback, useState } from "react";

/**
 * Hook obsługujący pobieranie plików
 * @param options Opcje konfiguracyjne
 * @returns Funkcje i stan do obsługi pobierania
 */
export const useFileDownloader = () => {
  const [isDownloading, setIsDownloading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const downloadFile = useCallback(
    async (
      fetchFunction: () => Promise<{
        data: Blob | null;
        fileName: string | null;
        mimeType?: string | null;
        error: string | null;
      }>,
      onSuccess?: () => void,
      onError?: (error: string) => void,
    ) => {
      setIsDownloading(true);
      setError(null);

      try {
        const { data, fileName, mimeType, error } = await fetchFunction();

        if (error || !data) {
          const errorMessage = error ?? "Nie udało się pobrać pliku.";
          setError(errorMessage);
          onError?.(errorMessage);
          return;
        }

        const blob = new Blob([data], { type: mimeType || "text/csv" });
        const url = window.URL.createObjectURL(blob);

        const link = document.createElement("a");
        link.href = url;
        link.setAttribute("download", fileName || "pobrano_plik.csv");
        link.style.display = "none";
        document.body.appendChild(link);

        link.click();

        setTimeout(() => {
          document.body.removeChild(link);
          window.URL.revokeObjectURL(url);
        }, 100);

        onSuccess?.();
      } catch (err) {
        console.error(err);
        const errorMessage = "Wystąpił nieoczekiwany błąd podczas pobierania.";
        setError(errorMessage);
        onError?.(errorMessage);
      } finally {
        setIsDownloading(false);
      }
    },
    [],
  );

  return {
    downloadFile,
    isDownloading,
    error,
  };
};
