import Link from "next/link";
import { FaGraduationCap } from "react-icons/fa";
import React from "react";

export const Header = () => {
  return (
    <header className="flex flex-col items-center justify-between gap-4 bg-darkNavy p-5 md:flex-row md:px-10">
      <Link href="/">
        <div className="flex items-center gap-2 leading-none text-sky-500">
          <FaGraduationCap className="text-[40px] md:text-[48px]" />
          <div className="hidden flex-col justify-center md:flex">
            <span className="text-[20px] font-bold leading-[1.1] md:text-[24px]">
              University
            </span>
            <span className="text-[20px] font-bold leading-[1.1] md:text-[24px]">
              Pilot
            </span>
          </div>
        </div>
      </Link>
      <Link
        href="/login"
        className="text-lg font-semibold text-gray-100 transition hover:text-green-600 hover:underline md:text-xl"
      >
        Zaloguj się
      </Link>
    </header>
  );
};
