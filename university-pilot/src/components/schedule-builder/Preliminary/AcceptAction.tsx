"use client";
import React, { useState } from "react";
import { Button } from "@/components/ui/Button";
import { toast } from "react-toastify";
import { getRandomLoadingMessage } from "@/utils/getRandomLoadingMessage";
import { acceptWeekendAvailability } from "@/lib/api/schedule-builder/acceptWeekendAvailability";
import ConfirmModal from "@/components/schedule-builder/Preliminary/ConfirmModal";
import { useRouter } from "next/navigation";
import { acceptSchedule } from "@/lib/api/schedule-builder/acceptSchedule";

interface Props {
  id: number;
  stage: number;
}

export default function AcceptAction({ id, stage }: Props) {
  const router = useRouter();
  const [showModal, setShowModal] = useState(false);

  async function handleSubmit() {
    if (!id) {
      return toast.error("Błąd identyfikatora semestru");
    }

    try {
      const { success, error } =
        stage === 2
          ? await acceptWeekendAvailability({
              semesterId: id,
            })
          : await acceptSchedule({ semesterId: id });

      if (!success) {
        toast.error(error ?? "Nie udało się zapisać.");
        return;
      }

      toast.success(getRandomLoadingMessage("success"));

      router.push(
        stage === 2
          ? "/dashboard/schedule-builder/final"
          : "/dashboard/calendar",
      );
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
          {stage === 2 ? "Akceptuj i generuj" : "Akceptuj harmonogram"}
        </Button>
      </div>

      {showModal && (
        <ConfirmModal
          title="Potwierdzenie akcji"
          description={
            stage === 2
              ? "Czy na pewno chcesz zaakceptować dostępność i rozpocząć generowanie harmonogramu?"
              : "Czy na pewno chcesz zaakceptować harmonogram?"
          }
          onCancel={() => setShowModal(false)}
          onConfirm={handleSubmit}
        />
      )}
    </>
  );
}
