import { getFileTypes } from "@/lib/api/fetchFileTypes";
import CsvUploadClient from "@/components/data-manager/CsvUploadClient";
import CsvDownloadClient from "@/components/data-manager/CsvDownloadClient";

export default async function CsvUploadForm() {
  const { data, error } = await getFileTypes();

  if (!data || error) {
    return (
      <div className="font-semibold text-red-500">
        Błąd podczas pobierania typów plików: {error}
      </div>
    );
  }

  const datasets = Object.entries(data).map(([value, label]) => ({
    value,
    label,
  }));

  return (
    <div className="mx-auto w-full max-w-6xl px-4 py-8">
      <div className="grid grid-cols-1 gap-6 md:grid-cols-2">
        <CsvUploadClient datasets={datasets} />
        <CsvDownloadClient />
      </div>
    </div>
  );
}
