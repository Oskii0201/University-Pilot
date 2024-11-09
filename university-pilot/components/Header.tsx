"use client";
import { logout } from "@/app/lib/auth";
import React from "react";
const Header = () => {
  return (
    <header className="flex items-center justify-between bg-lightGray p-4 text-darkGray shadow">
      <h1 className="text-xl font-semibold">University Pilot</h1>
      <div>
        <span className="mr-4">
          Zalogowany jako: <strong>Administrator</strong>
        </span>
        <button
          onClick={() => logout()}
          className="rounded bg-red-500 px-4 py-2 text-offWhite hover:bg-red-600"
        >
          Wyloguj się
        </button>
      </div>
    </header>
  );
};

export default Header;
