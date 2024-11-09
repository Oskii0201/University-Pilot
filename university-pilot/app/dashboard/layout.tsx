import Sidebar from "@/components/Sidebar/Sidebar";
import Header from "@/components/Header";
import React from "react";

interface LayoutProps {
  children: React.ReactNode;
}

function Layout({ children }: LayoutProps) {
  return (
    <div className="flex flex-1">
      <Sidebar />
      <div className="ml-24 flex flex-1 flex-col md:ml-48 lg:ml-72">
        <Header />
        <main className="flex-1 p-6">{children}</main>
      </div>
    </div>
  );
}

export default Layout;
