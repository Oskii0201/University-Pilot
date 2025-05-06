"use client";
import { FaHome, FaCalendarAlt, FaChalkboard } from "react-icons/fa";
import Link from "next/link";
import { usePathname } from "next/navigation";
import React from "react";
import { BsDatabaseFillAdd } from "react-icons/bs";

type NavItem = {
  icon: JSX.Element;
  label: string;
  href: string;
};

interface NavigationProps {
  onLinkClick?: () => void;
}

const navItems: NavItem[] = [
  { icon: <FaHome size={20} />, label: "Dashboard", href: "/dashboard" },
  {
    icon: <FaChalkboard size={20} />,
    label: "Harmonogram",
    href: "/dashboard/schedule-builder",
  },
  {
    icon: <FaCalendarAlt size={20} />,
    label: "Kalendarz",
    href: "/dashboard/calendar",
  },
  {
    icon: <BsDatabaseFillAdd size={20} />,
    label: "Manager danych",
    href: "/dashboard/data-manager",
  },
];

const Navigation: React.FC<NavigationProps> = ({ onLinkClick }) => {
  const pathname = usePathname();

  return (
    <nav>
      <ul className="flex flex-col gap-3">
        {navItems.map((item) => {
          const isActive = pathname === item.href;

          return (
            <li key={item.href}>
              <Link
                href={item.href}
                aria-label={item.label}
                onClick={onLinkClick}
                className={`group flex items-center gap-3 rounded-lg px-3 py-2.5 transition-all duration-200 hover:bg-sky-800/30 ${
                  isActive
                    ? "bg-sky-800/50 text-sky-400"
                    : "text-offWhite hover:text-sky-300"
                }`}
              >
                <span
                  className={`transition-transform duration-200 group-hover:scale-110 ${isActive ? "text-sky-400" : ""}`}
                >
                  {item.icon}
                </span>
                <span className="font-medium">{item.label}</span>
              </Link>
            </li>
          );
        })}
      </ul>
    </nav>
  );
};

export default Navigation;
