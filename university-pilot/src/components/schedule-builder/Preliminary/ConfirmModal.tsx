"use client";
import React from "react";
import { Button } from "@/components/ui/Button";

interface ConfirmModalProps {
  title: string;
  description: string;
  onCancel: () => void;
  onConfirm: () => void;
}

export default function ConfirmModal({
  title,
  description,
  onCancel,
  onConfirm,
}: ConfirmModalProps) {
  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/50">
      <div className="w-full max-w-md rounded-2xl bg-white p-6 shadow-xl">
        <h2 className="mb-4 text-xl font-bold">{title}</h2>
        <p className="mb-6 text-gray-700">{description}</p>
        <div className="flex justify-end gap-3">
          <Button onClick={onCancel} color="grey">
            Anuluj
          </Button>
          <Button onClick={onConfirm} color="blue">
            Potwierdź
          </Button>
        </div>
      </div>
    </div>
  );
}
