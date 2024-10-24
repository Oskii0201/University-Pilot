"use client";
import {
  FaHome,
  FaCalendarAlt,
  FaChalkboard,
  FaUsers,
  FaBook,
  FaUserTie,
} from "react-icons/fa";
import Link from "next/link";
import { motion } from "framer-motion";
import { usePathname } from "next/navigation";

type NavItem = {
  icon: JSX.Element;
  label: string;
  href: string;
};

const navItems: NavItem[] = [
  { icon: <FaHome />, label: "Dashboard", href: "/dashboard" },
  { icon: <FaCalendarAlt />, label: "Harmonogramy", href: "/schedules" },
  { icon: <FaChalkboard />, label: "Sale", href: "/rooms" },
  { icon: <FaUsers />, label: "Grupy", href: "/groups" },
  { icon: <FaBook />, label: "Przedmioty", href: "/subjects" },
  { icon: <FaUserTie />, label: "Wykładowcy", href: "/lecturers" },
];

const Navigation: React.FC = () => {
  const pathname = usePathname();

  return (
    <ul className="flex flex-col gap-4 text-lg">
      {navItems.map((item) => {
        const isActive = pathname === item.href;
        const activeLinkClass = isActive ? "text-sky-400" : "text-offWhite";

        return (
          <motion.li
            className={`font-bold ${activeLinkClass}`}
            key={item.href}
            animate="visible"
            initial="hidden"
            whileHover={{ scale: 1.1, color: "rgb(14 165 233)" }}
            whileTap={{ scale: 0.9 }}
          >
            <Link
              href={item.href}
              aria-label={item.label}
              className={`flex items-center justify-center space-x-2 p-2 md:justify-start ${activeLinkClass}`}
            >
              <span>{item.icon}</span>
              <span className="hidden md:block">{item.label}</span>
            </Link>
          </motion.li>
        );
      })}
    </ul>
  );
};
export default Navigation;