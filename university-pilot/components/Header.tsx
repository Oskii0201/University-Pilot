"use client";
import React, { useState } from "react";
import { FaBars } from "react-icons/fa";
import { IoMdArrowDropdown } from "react-icons/io";
import { useClickOutside } from "@/hooks/useClickOutside";
import { logout } from "@/app/lib/auth";

interface HeaderProps {
  onHamburgerClick: () => void;
}

const Header: React.FC<HeaderProps> = ({ onHamburgerClick }) => {
  const [isDropdownOpen, setDropdownOpen] = useState(false);

  const closeDropdown = () => setDropdownOpen(false);

  const dropdownRef = useClickOutside(closeDropdown);

  return (
    <header className="flex items-center justify-between bg-lightGray p-4 text-darkGray shadow md:px-6 lg:px-8">
      <div className="flex items-center gap-4">
        <button
          onClick={onHamburgerClick}
          className="text-darkGray md:hidden"
          aria-label="Open Sidebar"
        >
          <FaBars size={24} />
        </button>
        <h1 className="text-lg font-semibold sm:text-xl md:text-2xl">
          University Pilot
        </h1>
      </div>

      <div className="relative" ref={dropdownRef}>
        <button
          onClick={() => setDropdownOpen((prev) => !prev)}
          className="flex items-center gap-2 rounded px-4 py-2 text-sm font-semibold text-darkGray hover:bg-gray-200"
        >
          Administrator
          <IoMdArrowDropdown size={20} />
        </button>

        {isDropdownOpen && (
          <div className="absolute right-0 z-40 mt-2 w-48 rounded bg-white p-2 shadow-lg">
            <ul className="text-sm">
              <li>
                <button
                  className="block w-full px-4 py-2 text-left hover:bg-gray-100"
                  onClick={() => alert("Przejście do profilu")}
                >
                  Mój profil
                </button>
              </li>
              <li>
                <button
                  className="block w-full px-4 py-2 text-left hover:bg-gray-100"
                  onClick={() => alert("Ustawienia")}
                >
                  Ustawienia
                </button>
              </li>
              <li>
                <button
                  className="block w-full px-4 py-2 text-left text-red-500 hover:bg-gray-100"
                  onClick={() => logout()}
                >
                  Wyloguj się
                </button>
              </li>
            </ul>
          </div>
        )}
      </div>
    </header>
  );
};

export default Header;
