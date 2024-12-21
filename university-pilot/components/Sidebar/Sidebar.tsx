"use client";
import Navigation from "@/components/Sidebar/Navigation";
import { motion } from "framer-motion";
import { FaTimes } from "react-icons/fa";

interface SidebarProps {
  isOpen: boolean;
  onClose: () => void;
}

const Sidebar: React.FC<SidebarProps> = ({ isOpen, onClose }) => {
  return (
    <>
      <aside className="fixed inset-y-0 left-0 z-40 hidden w-48 bg-darkNavy p-6 text-offWhite md:block lg:w-72">
        <div className="mb-8 text-2xl text-sky-400">[Logo]</div>
        <Navigation />
      </aside>

      <motion.aside
        initial={{ x: "-100%" }}
        animate={{ x: isOpen ? 0 : "-100%" }}
        transition={{ type: "spring", stiffness: 260, damping: 20 }}
        className="fixed inset-y-0 left-0 z-50 w-[30vw] max-w-[240px] bg-darkNavy p-4 text-offWhite shadow-lg md:hidden"
      >
        <button
          onClick={onClose}
          className="absolute right-4 top-4 text-offWhite"
          aria-label="Zamknij sidebar"
        >
          <FaTimes size={24} />
        </button>

        <div className="flex h-screen flex-col justify-center">
          <Navigation onLinkClick={onClose} />
        </div>
      </motion.aside>

      {isOpen && (
        <div
          className="fixed inset-0 z-40 bg-black bg-opacity-50 md:hidden"
          onClick={onClose}
        />
      )}
    </>
  );
};

export default Sidebar;
