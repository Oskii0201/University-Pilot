import Navigation from "@/components/Sidebar/Navigation";
import { FaGraduationCap, FaTimes } from "react-icons/fa";
import React from "react";

interface SidebarProps {
  isOpen: boolean;
  onClose: () => void;
}

const Sidebar: React.FC<SidebarProps> = ({ isOpen, onClose }) => {
  return (
    <>
      <aside className="fixed inset-y-0 left-0 z-40 hidden w-60 bg-darkNavy p-6 text-offWhite shadow-md transition-all duration-300 md:block">
        <div className="mb-8 flex items-center gap-2">
          <FaGraduationCap className="text-[40px] text-sky-500 md:text-[48px]" />
          <div className="hidden flex-col justify-center md:flex">
            <span className="text-[20px] font-bold leading-[1.1] md:text-[24px]">
              University
            </span>
            <span className="text-[20px] font-bold leading-[1.1] md:text-[24px]">
              Pilot
            </span>
          </div>
        </div>
        <Navigation />
      </aside>

      <aside
        className={`fixed inset-y-0 left-0 z-50 w-[75vw] max-w-[280px] bg-darkNavy p-6 text-offWhite shadow-lg transition-transform duration-300 ease-in-out md:hidden ${
          isOpen ? "translate-x-0" : "-translate-x-full"
        }`}
      >
        <button
          onClick={onClose}
          className="absolute right-4 top-4 rounded-full bg-sky-800 p-2 text-offWhite transition-colors duration-200 hover:bg-sky-700"
          aria-label="Zamknij sidebar"
        >
          <FaTimes size={20} />
        </button>

        <div className="mt-12">
          <div className="mb-8 flex items-center gap-2">
            <FaGraduationCap className="text-[40px] text-sky-500 md:text-[48px]" />
            <div className="flex flex-col justify-center">
              <span className="text-[20px] font-bold leading-[1.1] md:text-[24px]">
                University
              </span>
              <span className="text-[20px] font-bold leading-[1.1] md:text-[24px]">
                Pilot
              </span>
            </div>
          </div>
          <Navigation onLinkClick={onClose} />
        </div>
      </aside>

      {isOpen && (
        <div
          className="fixed inset-0 z-40 bg-black/60 backdrop-blur-sm transition-opacity duration-300 md:hidden"
          onClick={onClose}
        />
      )}
    </>
  );
};

export default Sidebar;
