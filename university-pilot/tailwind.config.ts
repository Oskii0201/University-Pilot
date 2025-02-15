import type { Config } from "tailwindcss";

import tailwind_scrollbar from "tailwind-scrollbar";

const config: Config = {
  content: [
    "./pages/**/*.{js,ts,jsx,tsx,mdx}",
    "./components/**/*.{js,ts,jsx,tsx,mdx}",
    "./app/**/*.{js,ts,jsx,tsx,mdx}",
    "./__tests__/**/*.{js,ts,jsx,tsx}",
  ],
  theme: {
    extend: {
      colors: {
        lightGray: "var(--background)",
        darkGray: "var(--foreground)",
        veryDarkGray: "#1A1A1A",
        offWhite: "#FFFFFF",

        mutedBlue: "#64748B",
        softBlue: "#3275e5",
        darkBlue: "#2563eb",
        darkMutedBlue: "#4B5563",
        darkNavy: "#334155",

        softGreen: "#16a34a",
        mutedGreen: "#15803d",
        softRed: "#E74C3C",
        mutedRed: "#f53925",
        mutedGold: "#F1C40F",

        hoverLightGray: "#E5E5E5",
        hoverDarkGray: "#2C2C2C",
      },
      fontFamily: {
        sans: ["var(--font-geist-sans)"],
      },
    },
  },
  plugins: [tailwind_scrollbar],
};
export default config;
