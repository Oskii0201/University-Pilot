"use client";
import React, { useState } from "react";
import { Button } from "@/components/ui/Button";
import { toast } from "react-toastify";
import { getRandomLoadingMessage } from "@/utils/getRandomLoadingMessage";
import { acceptWeekendAvailability } from "@/lib/api/schedule-builder/acceptWeekendAvailability";
import ConfirmModal from "@/components/schedule-builder/Preliminary/ConfirmModal";
import { useRouter } from "next/navigation";

interface Props {
  id: number;
}

export default function AcceptAction({ id }: Props) {
  const router = useRouter();
  const [showModal, setShowModal] = useState(false);

  async function handleSubmit() {
    if (!id) {
      return toast.error("Błąd identyfikatora semestru");
    }

    try {
      const { success, error } = await acceptWeekendAvailability({
        semesterId: id,
      });

      if (!success) {
        toast.error(error ?? "Nie udało się zapisać.");
        return;
      }

      toast.success(getRandomLoadingMessage("success"));

      router.push("/dashboard/schedule-builder/final");
    } catch (error) {
      toast.error(getRandomLoadingMessage("error"));
      console.error(error);
    } finally {
      setShowModal(false);
    }
  }

  return (
    <>
      <div className="flex w-full justify-end">
        <Button width="w-fit" color="blue" onClick={() => setShowModal(true)}>
          Akceptuj i generuj
        </Button>
      </div>

      {showModal && (
        <ConfirmModal
          title="Potwierdzenie akcji"
          description="Czy na pewno chcesz zaakceptować dostępność i rozpocząć generowanie harmonogramu?"
          onCancel={() => setShowModal(false)}
          onConfirm={handleSubmit}
        />
      )}
    </>
  );
}
