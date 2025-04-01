import { getFileTypes } from "@/lib/api/fetchFileTypes";
import CsvUploadClient from "@/components/CsvUploadClient";

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

  return <CsvUploadClient datasets={datasets} />;
}
