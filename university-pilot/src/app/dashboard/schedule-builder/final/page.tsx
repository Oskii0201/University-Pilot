"use client";

import React, { useEffect, useState } from "react";
import ScheduleBuilderNavigation from "@/components/schedule-builder/ScheduleBuilderNavigation";
import SemesterList from "@/components/schedule-builder/SemesterList";
import Link from "next/link";
import { PiFolderSimplePlusFill } from "react-icons/pi";
import { getUpcomingSemestersAfterGroupSchedule } from "@/lib/api/schedule-builder/getUpcomingSemestersAfterGroupSchedule";
/*
TODO
 *  Posprzątać
 *  Dostosować widok listy i zablokować akcje
 *  Dodać creationStage do listy i jakiś komunikat o generowaniu
*/
const ScheduleBuilderFinalPage = () => {
  const [data, setData] = useState([]);
  const [error, setError] = useState(null);
  const [isLoading, setIsLoading] = useState(false);

  useEffect(() => {
    let intervalId = null;

    const fetchData = async () => {
      setIsLoading(true);

      try {
        const { data: newData, error: newError } =
          await getUpcomingSemestersAfterGroupSchedule();

        if (newError) {
          setError(newError);
          if (intervalId) {
            clearInterval(intervalId);
            intervalId = null;
          }
        } else {
          setData(newData || []);
          setError(null);

          const needsPolling = newData?.some(
            (item) => item.creationStage === 3 || item.creationStage === 4,
          );

          if (needsPolling && !intervalId) {
            intervalId = setInterval(fetchData, 60000);
          } else if (!needsPolling && intervalId) {
            clearInterval(intervalId);
            intervalId = null;
          }
        }
      } catch (e) {
        setError(e instanceof Error ? e.message : "Nieznany błąd");
        if (intervalId) {
          clearInterval(intervalId);
          intervalId = null;
        }
      } finally {
        setIsLoading(false);
      }
    };

    fetchData();

    return () => {
      if (intervalId) {
        clearInterval(intervalId);
      }
    };
  }, []);

  return (
    <div className="flex flex-col gap-4">
      <ScheduleBuilderNavigation />

      <div className="flex items-center justify-between">
        <h1 className="text-2xl font-bold">Generowane harmonogramy</h1>
        <Link
          href="/dashboard/schedule-builder/preliminary/new"
          aria-label="Przypisz weekendy"
          title="Przypisz weekendy"
          className="text-3xl text-softGreen transition hover:scale-110 hover:text-mutedGreen"
        >
          <PiFolderSimplePlusFill className="cursor-pointer" />
        </Link>
      </div>

      {isLoading && <div className="text-gray-600">Ładowanie...</div>}

      {error ? (
        <div className="rounded bg-red-100 p-4 text-red-700 shadow">
          <p className="font-semibold">Błąd:</p>
          <p>{error}</p>
        </div>
      ) : (
        <SemesterList
          groupSets={data}
          basePath="/dashboard/schedule-builder/preliminary"
        />
      )}

      {!isLoading && !error && data.length === 0 && (
        <div className="p-4 text-gray-600">Brak harmonogramów.</div>
      )}
    </div>
  );
};

export default ScheduleBuilderFinalPage;
