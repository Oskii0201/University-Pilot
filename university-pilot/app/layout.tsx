import type { Metadata } from "next";
import "./globals.css";
import Footer from "@/components/Footer";
import Header from "@/components/Header";
import Sidebar from "@/components/Sidebar/Sidebar";

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
        <div className="flex flex-1">
          <Sidebar />
          <div className="ml-24 flex flex-1 flex-col md:ml-48 lg:ml-72">
            <Header />

            <main className="ml-24 flex-1 p-6">{children}</main>

            <Footer />
          </div>
        </div>
      </body>
    </html>
  );
}
