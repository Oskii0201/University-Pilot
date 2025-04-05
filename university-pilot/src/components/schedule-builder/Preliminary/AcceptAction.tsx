"use client";
import React from "react";
import { Button } from "@/components/ui/Button";
import { toast } from "react-toastify";
import { getRandomLoadingMessage } from "@/utils/getRandomLoadingMessage";
import { acceptWeekendAvailability } from "@/lib/api/schedule-builder/acceptWeekendAvailability";

interface Props {
  id: number;
}

/*TODO
 *  Dodać potwierdzenie wykonania akceptacji
 *  Dodać odświeżenie listy semestrów*/
export default function AcceptAction({ id }: Props) {
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
    } catch (error) {
      toast.error(getRandomLoadingMessage("error"));
      console.error(error);
    }
  }

  return (
    <div className="flex w-full justify-end">
      <Button width="w-fit" color="blue" onClick={handleSubmit}>
        Akceptuj i generuj
      </Button>
    </div>
  );
}
