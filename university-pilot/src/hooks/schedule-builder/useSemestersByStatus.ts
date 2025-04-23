import { useState, useCallback, useEffect } from "react";
import { toast } from "react-toastify";
import { FieldOfStudy, Semester } from "@/app/types";
import { getSemestersByStatus } from "@/lib/api/schedule-builder/getSemestersByStatus";
import { getFieldsOfStudy } from "@/lib/api/schedule-builder/getFieldsOfStudy";

export const useSemestersByStatus = (
  stageID: number,
  initialSemesterID?: number,
) => {
  const [semesters, setSemesters] = useState<Semester[]>([]);
  const [selectedSemester, setSelectedSemester] = useState<Semester | null>(
    null,
  );
  const [fieldsOfStudy, setFieldsOfStudy] = useState<FieldOfStudy[]>([]);
  const [selectedFieldOfStudy, setSelectedFieldOfStudy] =
    useState<FieldOfStudy | null>(null);
  const [selectedSemesterNumber, setSelectedSemesterNumber] =
    useState<number>(null);

  const [isLoading, setIsLoading] = useState(true);

  const fetchSemesters = useCallback(async () => {
    try {
      setIsLoading(true);

      const { data, error } = await getSemestersByStatus(stageID);

      if (error || !data) {
        toast.error("Nie udało się pobrać semestrów");
        return;
      }

      setSemesters(data);

      if (initialSemesterID) {
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
  }, [stageID, initialSemesterID]);

  const fetchFieldsOfStudy = useCallback(async () => {
    if (!selectedSemester?.id) {
      return;
    }

    try {
      setIsLoading(true);

      const { data, error } = await getFieldsOfStudy(selectedSemester.id);

      if (error || !data) {
        toast.error("Nie udało się pobrać kierunków");
        return;
      }

      setFieldsOfStudy(data);
    } catch (error) {
      toast.error("Wystąpił błąd przy pobieraniu kierunków");
      console.error(error);
    } finally {
      setIsLoading(false);
    }
  }, [selectedSemester]);

  const handleSemesterChange = useCallback(
    async (selectedOption: { value: number } | null) => {
      if (!selectedOption) return null;

      const semester =
        semesters.find((s) => s.id === selectedOption.value) || null;
      setSelectedSemester(semester);
      setSelectedFieldOfStudy(null);
      setSelectedSemesterNumber(null);
      return semester;
    },
    [semesters],
  );

  const handleFieldOfStudyChange = useCallback(
    async (selectedOption: { value: number } | null) => {
      if (!selectedOption) return null;

      const fieldOfStudy =
        fieldsOfStudy.find((s) => s.id === selectedOption.value) || null;
      setSelectedFieldOfStudy(fieldOfStudy);
      return fieldOfStudy;
    },
    [fieldsOfStudy],
  );

  const handleSemesterNumberChange = useCallback(
    async (selectedOption: { value: number } | null) => {
      if (!selectedOption) return null;

      setSelectedSemesterNumber(selectedOption.value);
    },
    [],
  );

  useEffect(() => {
    fetchSemesters();
  }, [fetchSemesters]);

  useEffect(() => {
    fetchFieldsOfStudy();
  }, [fetchFieldsOfStudy]);

  return {
    semesters,
    selectedSemester,
    fieldsOfStudy,
    selectedFieldOfStudy,
    selectedSemesterNumber,
    isLoading,
    fetchSemesters,
    fetchFieldsOfStudy,
    handleSemesterChange,
    handleFieldOfStudyChange,
    handleSemesterNumberChange,
  };
};
