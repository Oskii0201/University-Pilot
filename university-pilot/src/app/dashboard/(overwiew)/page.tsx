"use client";
import { useState } from "react";
import { FaCalendarAlt, FaUsers, FaBookOpen, FaClock } from "react-icons/fa";
import { HiOutlineAcademicCap } from "react-icons/hi";
import { BsGraphUp } from "react-icons/bs";

export default function Dashboard() {
  const [isLoading] = useState(false);

  return (
    <div className="space-y-6 p-6">
      <h1 className="text-2xl font-bold text-gray-800">
        Dashboard University Pilot
      </h1>

      <div className="grid grid-cols-1 gap-4 md:grid-cols-2 lg:grid-cols-4">
        <div className="rounded-lg border-l-4 border-blue-500 bg-white p-4 shadow transition-transform hover:scale-105">
          <div className="flex items-center justify-between">
            <div>
              <p className="text-sm text-gray-500">Aktywne semestry</p>
              <p className="text-2xl font-bold">3</p>
            </div>
            <div className="rounded-full bg-blue-100 p-3">
              <HiOutlineAcademicCap className="text-xl text-blue-500" />
            </div>
          </div>
        </div>

        <div className="rounded-lg border-l-4 border-green-500 bg-white p-4 shadow transition-transform hover:scale-105">
          <div className="flex items-center justify-between">
            <div>
              <p className="text-sm text-gray-500">Zaplanowane zajęcia</p>
              <p className="text-2xl font-bold">128</p>
            </div>
            <div className="rounded-full bg-green-100 p-3">
              <FaCalendarAlt className="text-xl text-green-500" />
            </div>
          </div>
        </div>

        <div className="rounded-lg border-l-4 border-purple-500 bg-white p-4 shadow transition-transform hover:scale-105">
          <div className="flex items-center justify-between">
            <div>
              <p className="text-sm text-gray-500">Programy studiów</p>
              <p className="text-2xl font-bold">12</p>
            </div>
            <div className="rounded-full bg-purple-100 p-3">
              <FaBookOpen className="text-xl text-purple-500" />
            </div>
          </div>
        </div>

        <div className="rounded-lg border-l-4 border-amber-500 bg-white p-4 shadow transition-transform hover:scale-105">
          <div className="flex items-center justify-between">
            <div>
              <p className="text-sm text-gray-500">Wykorzystanie zasobów</p>
              <p className="text-2xl font-bold">76%</p>
            </div>
            <div className="rounded-full bg-amber-100 p-3">
              <BsGraphUp className="text-xl text-amber-500" />
            </div>
          </div>
        </div>
      </div>

      <div className="rounded-lg bg-white p-5 shadow">
        <h2 className="mb-4 text-xl font-semibold">Aktualny semestr</h2>
        {isLoading ? (
          <div className="flex justify-center py-8">
            <div className="h-8 w-8 animate-spin rounded-full border-b-2 border-blue-500"></div>
          </div>
        ) : (
          <div className="grid grid-cols-1 gap-6 md:grid-cols-2">
            <div className="space-y-4">
              <div className="flex items-center gap-3">
                <div className="rounded-full bg-blue-100 p-2">
                  <FaClock className="text-blue-500" />
                </div>
                <div>
                  <p className="text-sm text-gray-500">Nazwa semestru</p>
                  <p className="font-medium">Semestr letni 2024/2025</p>
                </div>
              </div>

              <div className="flex items-center gap-3">
                <div className="rounded-full bg-green-100 p-2">
                  <FaCalendarAlt className="text-green-500" />
                </div>
                <div>
                  <p className="text-sm text-gray-500">Okres</p>
                  <p className="font-medium">01.03.2025 - 30.06.2025</p>
                </div>
              </div>

              <div className="flex items-center gap-3">
                <div className="rounded-full bg-purple-100 p-2">
                  <FaUsers className="text-purple-500" />
                </div>
                <div>
                  <p className="text-sm text-gray-500">Liczba grup</p>
                  <p className="font-medium">24</p>
                </div>
              </div>
            </div>

            <div className="rounded-lg bg-gray-50 p-4">
              <h3 className="mb-3 font-medium">Postęp planowania</h3>
              <div className="space-y-3">
                <div>
                  <div className="mb-1 flex justify-between text-sm">
                    <span>Harmonogram zajęć</span>
                    <span className="font-medium">85%</span>
                  </div>
                  <div className="h-2 w-full rounded-full bg-gray-200">
                    <div
                      className="h-2 rounded-full bg-blue-500"
                      style={{ width: "85%" }}
                    ></div>
                  </div>
                </div>

                <div>
                  <div className="mb-1 flex justify-between text-sm">
                    <span>Przydzielenie sal</span>
                    <span className="font-medium">72%</span>
                  </div>
                  <div className="h-2 w-full rounded-full bg-gray-200">
                    <div
                      className="h-2 rounded-full bg-green-500"
                      style={{ width: "72%" }}
                    ></div>
                  </div>
                </div>

                <div>
                  <div className="mb-1 flex justify-between text-sm">
                    <span>Przydzielenie prowadzących</span>
                    <span className="font-medium">91%</span>
                  </div>
                  <div className="h-2 w-full rounded-full bg-gray-200">
                    <div
                      className="h-2 rounded-full bg-purple-500"
                      style={{ width: "91%" }}
                    ></div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        )}
      </div>

      <div className="grid grid-cols-1 gap-6 lg:grid-cols-2">
        <div className="rounded-lg bg-white p-5 shadow">
          <h2 className="mb-4 text-xl font-semibold">Nadchodzące semestry</h2>
          <div className="space-y-3">
            {isLoading ? (
              <div className="flex justify-center py-6">
                <div className="h-8 w-8 animate-spin rounded-full border-b-2 border-blue-500"></div>
              </div>
            ) : (
              <>
                <div className="flex items-center justify-between rounded-lg border-l-4 border-blue-500 bg-gray-50 p-3">
                  <div>
                    <p className="font-medium">Semestr zimowy 2025/2026</p>
                    <p className="text-sm text-gray-500">
                      01.10.2025 - 15.02.2026
                    </p>
                  </div>
                  <span className="rounded-full bg-blue-100 px-2 py-1 text-xs font-medium text-blue-800">
                    Planowany
                  </span>
                </div>

                <div className="flex items-center justify-between rounded-lg border-l-4 border-amber-500 bg-gray-50 p-3">
                  <div>
                    <p className="font-medium">Semestr letni 2025/2026</p>
                    <p className="text-sm text-gray-500">
                      01.03.2026 - 30.06.2026
                    </p>
                  </div>
                  <span className="rounded-full bg-amber-100 px-2 py-1 text-xs font-medium text-amber-800">
                    Przygotowanie
                  </span>
                </div>
              </>
            )}
          </div>
        </div>

        <div className="rounded-lg bg-white p-5 shadow">
          <h2 className="mb-4 text-xl font-semibold">Szybkie akcje</h2>
          <div className="grid grid-cols-2 gap-3">
            <button className="flex flex-col items-center justify-center rounded-lg border border-blue-100 bg-blue-50 p-4 transition-colors hover:bg-blue-100">
              <FaCalendarAlt className="mb-2 text-xl text-blue-500" />
              <span className="text-sm font-medium">Dodaj zajęcia</span>
            </button>

            <button className="flex flex-col items-center justify-center rounded-lg border border-green-100 bg-green-50 p-4 transition-colors hover:bg-green-100">
              <FaUsers className="mb-2 text-xl text-green-500" />
              <span className="text-sm font-medium">Zarządzaj grupami</span>
            </button>

            <button className="flex flex-col items-center justify-center rounded-lg border border-purple-100 bg-purple-50 p-4 transition-colors hover:bg-purple-100">
              <FaBookOpen className="mb-2 text-xl text-purple-500" />
              <span className="text-sm font-medium">Programy studiów</span>
            </button>

            <button className="flex flex-col items-center justify-center rounded-lg border border-amber-100 bg-amber-50 p-4 transition-colors hover:bg-amber-100">
              <BsGraphUp className="mb-2 text-xl text-amber-500" />
              <span className="text-sm font-medium">Raporty</span>
            </button>
          </div>
        </div>
      </div>
    </div>
  );
}
