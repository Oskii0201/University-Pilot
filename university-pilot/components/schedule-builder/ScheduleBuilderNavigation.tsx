import { Button } from "@/components/ui/Button";
import React from "react";

const ScheduleBuilderNavigation = () => {
  const tabs = [
    { label: "Semestry", href: "/dashboard/schedule-builder/groups" },
    {
      label: "Wstępny harmonogram",
      href: "/dashboard/schedule-builder/preliminary",
    },
    { label: "Finalny harmonogram", href: "/dashboard/schedule-builder/final" },
  ];

  return (
    <div className="mb-4 flex flex-wrap gap-2 border-b pb-2">
      {tabs.map((tab) => (
        <Button
          key={tab.href}
          href={tab.href}
          width="w-fit"
          color="grey"
          bold={false}
        >
          {tab.label}
        </Button>
      ))}
    </div>
  );
};

export default ScheduleBuilderNavigation;
