import type { Metadata } from "next";
import "./globals.css";
import "react-toastify/dist/ReactToastify.css";
import Footer from "@/components/Footer";
import React from "react";
import { ToastContainer } from "react-toastify";

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
      <body className="flex min-h-screen flex-col antialiased">
        <ToastContainer
          position="top-center"
          autoClose={5000}
          hideProgressBar={false}
          newestOnTop
          closeOnClick
          pauseOnHover
          draggable
        />
        <main className="flex flex-1 justify-center">{children}</main>
        <Footer />
      </body>
    </html>
  );
}
