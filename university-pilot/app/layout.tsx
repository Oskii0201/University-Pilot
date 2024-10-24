import type { Metadata } from "next";
import "./globals.css";

export const metadata: Metadata = {
  title: "University Pilot",
  description: "Automated schedules for your university",
};

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="en">
      <body className={`antialiased`}>{children}</body>
    </html>
  );
}
