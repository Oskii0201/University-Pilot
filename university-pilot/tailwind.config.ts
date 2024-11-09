import type { Config } from "tailwindcss";

import tailwind_scrollbar from "tailwind-scrollbar";

const config: Config = {
  content: [
    "./pages/**/*.{js,ts,jsx,tsx,mdx}",
    "./components/**/*.{js,ts,jsx,tsx,mdx}",
    "./app/**/*.{js,ts,jsx,tsx,mdx}",
  ],
  theme: {
    extend: {
      colors: {
        lightGray: "var(--background)",
        darkGray: "var(--foreground)",
        veryDarkGray: "#1A1A1A",
        offWhite: "#FFFFFF",

        mutedBlue: "#64748B",
        darkMutedBlue: "#4B5563",
        darkNavy: "#334155",

        softGreen: "#22c55e",
        softRed: "#E74C3C",
        mutedGold: "#F1C40F",

        hoverLightGray: "#E5E5E5",
        hoverDarkGray: "#2C2C2C",
      },
    },
  },
  plugins: [tailwind_scrollbar],
};
export default config;
