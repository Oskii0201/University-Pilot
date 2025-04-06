"use client";

import React, { useState } from "react";
import { Button } from "@/components/ui/Button";
import { toast } from "react-toastify";
import { exportCourseDetailsAsCSV } from "@/lib/api/data-manager/exportCourseDetailsAsCSV";
import { useFileDownloader } from "@/hooks/useFileDownloader";

export default function CsvDownloadClient() {
  const { downloadFile, isDownloading } = useFileDownloader();
  const [semesterId, setSemesterId] = useState<number | null>(null);

  const handleDownload = async () => {
    if (!semesterId || semesterId <= 0) {
      toast.error("Proszę podać poprawne ID semestru.");
      return;
    }

    await downloadFile(
      () => exportCourseDetailsAsCSV(semesterId),
      () => toast.success("Pobrano plik pomyślnie."),
      (err) => toast.error(err),
    );
  };

  return (
    <div className="relative mx-auto max-w-lg rounded-lg p-6 shadow-lg">
      <h2 className="mb-4 text-2xl font-bold">Pobierz CSV</h2>

      <div className="grid grid-cols-1 items-end gap-4 md:grid-cols-2">
        <div>
          <label htmlFor="fileId" className="mb-2 block font-medium">
            ID semestru
          </label>
          <input
            id="fileId"
            type="number"
            min={1}
            value={semesterId ?? ""}
            onChange={(e) => setSemesterId(Number(e.target.value))}
            className="w-full rounded border bg-offWhite p-2"
            disabled={isDownloading}
            required
          />
        </div>

        <Button
          onClick={handleDownload}
          color="blue"
          width="w-full"
          disabled={!semesterId || isDownloading}
        >
          {isDownloading ? "Pobieranie..." : "Pobierz"}
        </Button>
      </div>
    </div>
  );
}
