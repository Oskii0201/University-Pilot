import Sidebar from "@/components/Sidebar/Sidebar";
import Header from "@/components/Header";
import React, { Suspense } from "react";
import Loading from "@/app/dashboard/loading";

interface LayoutProps {
  children: React.ReactNode;
}

function Layout({ children }: LayoutProps) {
  return (
    <div className="flex flex-1">
      <Sidebar />
      <div className="ml-24 flex flex-1 flex-col md:ml-48 lg:ml-72">
        <Header />
        <Suspense fallback={<Loading />}>
          <main className="flex-1 p-6">{children}</main>
        </Suspense>
      </div>
    </div>
  );
}

export default Layout;
