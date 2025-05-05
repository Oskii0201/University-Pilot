import { useState, useCallback, useEffect } from "react";
import { Weekend, BasicGroup } from "@/app/types";
import { getWeekendAvailability } from "@/lib/api/schedule-builder/getWeekendAvailability";
import { toast } from "react-toastify";
import { getRandomLoadingMessage } from "@/utils/getRandomLoadingMessage";
import { updateWeekendAvailability } from "@/lib/api/schedule-builder/updateWeekendAvailability";
import { useRouter } from "next/navigation";

export const hasTrueValues = (schedule: Weekend[]) =>
  schedule.some(({ availability }) =>
    Object.values(availability).some(Boolean),
  );

export const useWeekendAvailability = (initialSemesterID?: number) => {
  const router = useRouter();
  const [schedule, setSchedule] = useState<Weekend[]>([]);
  const [groups, setGroups] = useState<BasicGroup[]>([]);
  const [isLoading, setIsLoading] = useState(false);

  const fetchWeekendAvailability = useCallback(async (semesterID: number) => {
    try {
      setIsLoading(true);

      const { data, error } = await getWeekendAvailability(semesterID);

      if (error || !data) {
        toast.error("Nie udało się pobrać harmonogramu weekendów");
        return;
      }

      setSchedule(data.weekends || []);
      setGroups(data.groups || []);
    } catch (error) {
      toast.error("Wystąpił błąd przy pobieraniu harmonogramu weekendów");
      console.error(error);
    } finally {
      setIsLoading(false);
    }
  }, []);

  useEffect(() => {
    if (initialSemesterID) {
      fetchWeekendAvailability(initialSemesterID);
    }
  }, [initialSemesterID, fetchWeekendAvailability]);

  const toggleAvailability = useCallback((date: string, groupId: number) => {
    setSchedule((prev) =>
      prev.map((weekend) =>
        weekend.date === date
          ? {
              ...weekend,
              availability: {
                ...weekend.availability,
                [groupId]: !weekend.availability[groupId],
              },
            }
          : weekend,
      ),
    );
  }, []);

  const hasUnsavedChanges = schedule.length > 0;

  const handleSubmit = async (semesterID: number) => {
    if (!semesterID) {
      return toast.error("Nie wybrano semestru");
    }

    try {
      setIsLoading(true);

      const payload = {
        semesterId: semesterID,
        groups: groups,
        weekends: schedule,
      };

      const { success, error } = await updateWeekendAvailability(payload);

      if (!success) {
        toast.error(error ?? "Nie udało się zapisać danych.");
        return;
      }

      toast.success(getRandomLoadingMessage("success"));
      router.push(`/dashboard/schedule-builder/preliminary/${semesterID}`);
    } catch (error) {
      toast.error(getRandomLoadingMessage("error"));
      console.error(error);
    } finally {
      setIsLoading(false);
    }
  };

  return {
    schedule,
    groups,
    isLoading,
    hasUnsavedChanges,
    fetchWeekendAvailability,
    toggleAvailability,
    hasTrueValues,
    handleSubmit,
  };
};
