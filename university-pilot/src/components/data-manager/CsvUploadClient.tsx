"use client";

import React, { useState } from "react";
import { Button } from "@/components/ui/Button";
import { toast } from "react-toastify";
import { LoadingCircle } from "@/components/ui/LoadingCircle";
import apiClient from "@/lib/apiClient";
import { getRandomLoadingMessage } from "@/utils/getRandomLoadingMessage";

interface Props {
  datasets: { value: string; label: string }[];
}

export default function CsvUploadClient({ datasets }: Props) {
  const [selectedDataset, setSelectedDataset] = useState("");
  const [file, setFile] = useState<File | null>(null);
  const [isUploading, setIsUploading] = useState(false);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!file || !selectedDataset) {
      toast.error(
        <div>
          <h2 className="font-semibold">Oops!</h2>
          <p>Proszę wybrać zbiór danych oraz dodać plik csv</p>
        </div>,
      );
      return;
    }

    setIsUploading(true);

    const formData = new FormData();
    formData.append("dataset", selectedDataset);
    formData.append("file", file);

    try {
      await apiClient.post("/file/upload", formData, {
        headers: { "Content-Type": "multipart/form-data" },
      });

      toast.success(getRandomLoadingMessage("success"));
    } catch (error) {
      toast.error(getRandomLoadingMessage("error"));
      console.error(error);
    } finally {
      setIsUploading(false);
      setSelectedDataset("");
      setFile(null);
    }
  };

  return (
    <div className="relative mx-auto max-w-lg rounded-lg p-6 shadow-lg">
      {isUploading && (
        <LoadingCircle
          isOverlay={true}
          message={getRandomLoadingMessage("upload")}
        />
      )}

      <h2 className="mb-4 text-2xl font-bold">Wgraj CSV</h2>

      <form
        onSubmit={handleSubmit}
        className={`${isUploading ? "pointer-events-none opacity-50" : ""}`}
      >
        <div className="mb-4">
          <label htmlFor="dataset" className="mb-2 block font-medium">
            Wybierz zbiór danych
          </label>
          <select
            id="dataset"
            value={selectedDataset}
            onChange={(e) => setSelectedDataset(e.target.value)}
            className="w-full rounded border bg-offWhite p-2"
            required
            disabled={isUploading}
          >
            <option value="">-- Zbiór danych --</option>
            {datasets.map((dataset) => (
              <option key={dataset.value} value={dataset.value}>
                {dataset.label}
              </option>
            ))}
          </select>
        </div>

        <div className="mb-4">
          <label htmlFor="file" className="mb-2 block font-medium">
            Dodaj plik csv
          </label>
          <input
            id="file"
            type="file"
            accept=".csv"
            onChange={(e) => setFile(e.target?.files?.[0] || null)}
            className="w-full rounded border bg-offWhite p-2"
            required
            disabled={isUploading}
          />
        </div>

        <Button
          color="blue"
          width="w-full"
          disabled={!file || !selectedDataset || isUploading}
        >
          Wyślij
        </Button>
      </form>
    </div>
  );
}
