import { useState, useCallback, useEffect } from "react";
import { toast } from "react-toastify";
import { Semester } from "@/app/types";
import { getUpcomingSemesters } from "@/lib/api/schedule-builder/getUpcomingSemesters";

export const useSemesters = (stageID: number, initialSemesterID?: number) => {
  const [semesters, setSemesters] = useState<Semester[]>([]);
  const [selectedSemester, setSelectedSemester] = useState<Semester | null>(
    null,
  );
  const [isLoading, setIsLoading] = useState(true);

  const fetchSemesters = useCallback(async () => {
    try {
      setIsLoading(true);

      if (!initialSemesterID) {
        const { data, error } = await getUpcomingSemesters(stageID);
        if (error || !data) {
          toast.error("Nie udało się pobrać semestrów");
          return;
        }
        setSemesters(data);
      }

      if (initialSemesterID) {
        const { data, error } = await getUpcomingSemesters(stageID + 1);
        if (error || !data) {
          toast.error("Nie udało się pobrać semestrów");
          return;
        }
        setSemesters(data);

        const initialSemester = data.find((s) => s.id === initialSemesterID);
        if (initialSemester) {
          setSelectedSemester(initialSemester);
        }
      }
    } catch (error) {
      toast.error("Wystąpił błąd przy pobieraniu semestrów");
      console.error(error);
    } finally {
      setIsLoading(false);
    }
  }, [initialSemesterID]);

  const handleSemesterChange = useCallback(
    async (
      selectedOption: { value: number } | null,
      hasUnsavedChanges: boolean,
    ) => {
      if (!selectedOption) return null;

      if (hasUnsavedChanges) {
        if (
          !window.confirm(
            "Zmiana semestru spowoduje utratę wszystkich danych. Kontynuować?",
          )
        ) {
          return null;
        }
      }

      const semester =
        semesters.find((s) => s.id === selectedOption.value) || null;
      setSelectedSemester(semester);
      return semester;
    },
    [semesters],
  );

  useEffect(() => {
    fetchSemesters();
  }, [fetchSemesters]);

  return {
    semesters,
    selectedSemester,
    isLoading,
    fetchSemesters,
    handleSemesterChange,
  };
};
