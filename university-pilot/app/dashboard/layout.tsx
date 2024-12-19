"use client";
import Sidebar from "@/components/Sidebar/Sidebar";
import Header from "@/components/Header";
import React, { useState, Suspense } from "react";
import Loading from "@/app/dashboard/loading";

interface LayoutProps {
  children: React.ReactNode;
}

function Layout({ children }: LayoutProps) {
  const [isSidebarOpen, setIsSidebarOpen] = useState(false);

  return (
    <div className="flex min-h-screen w-full">
      <Sidebar isOpen={isSidebarOpen} onClose={() => setIsSidebarOpen(false)} />

      <div className="flex flex-1 flex-col md:ml-48 lg:ml-72">
        <Header onHamburgerClick={() => setIsSidebarOpen(true)} />
        <Suspense fallback={<Loading />}>
          <main className="flex-1 p-4 md:p-6">
            <div className="w-full max-w-screen-2xl">{children}</div>
          </main>
        </Suspense>
      </div>
    </div>
  );
}

export default Layout;
