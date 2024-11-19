"use client";

import React, { useState } from "react";
import { useRouter } from "next/navigation";
import apiClient from "@/app/lib/apiClient";
import { Button } from "@/components/Button";
import { toast } from "react-toastify";

export default function CsvUploadForm() {
  const [selectedDataset, setSelectedDataset] = useState("");
  const [file, setFile] = useState<File | null>(null);
  const router = useRouter();

  const datasets = [
    { value: "studyProgram", label: "Program studiów" },
    { value: "historicalSchedule", label: "Harmonogram historyczny" },
    { value: "lecturers", label: "Wykładowcy" },
    { value: "students", label: "Studenci" },
  ];

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!file || !selectedDataset) {
      toast.error(
        <div>
          <h2 className="font-semibold"> Oops!</h2>
          <p>Proszę wybrać zbiór danych oraz dodać plik csv</p>
        </div>,
      );
    }

    const formData = new FormData();
    formData.append("dataset", selectedDataset);
    formData.append("file", file);
    try {
      await apiClient.post("/file/upload", formData, {
        headers: {
          "Content-Type": "multipart/form-data",
        },
      });

      toast.success(<p>Zakończono pomyślnie dodawanie pliku.</p>);
      router.push("/dashboard");
    } catch (error) {
      toast.error(
        <div>
          <h2 className="font-semibold"> Oops!</h2>
          <p>Nie udało się dodać pliku. Prosze spróbować ponownie</p>
          <p>{error}</p>
        </div>,
      );
    }
  };

  return (
    <div className="mx-auto max-w-lg rounded-lg p-6 shadow-lg">
      <h2 className="mb-4 text-2xl font-bold">Wgraj CSV</h2>

      <form onSubmit={handleSubmit}>
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
          >
            <option value="">-- Zbiór danych --</option>
            {datasets.map((dataset) => (
              <option key={dataset.value} value={dataset.value}>
                {dataset.label}
              </option>
            ))}
          </select>
        </div>

        {/* File Upload */}
        <div className="mb-4">
          <label htmlFor="file" className="mb-2 block font-medium">
            Dodaj plik csv
          </label>
          <input
            id="file"
            type="file"
            accept=".csv"
            onChange={(e) => setFile(e.target.files ? e.target.files[0] : null)}
            className="w-full rounded border bg-offWhite p-2"
            required
          />
        </div>

        <Button
          color="blue"
          width="w-full"
          disabled={!file || !selectedDataset}
        >
          Wyślij
        </Button>
      </form>
    </div>
  );
}
