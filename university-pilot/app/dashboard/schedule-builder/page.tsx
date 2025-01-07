import ScheduleBuilderNavigation from "@/components/schedule-builder/ScheduleBuilderNavigation";
import OverviewCard from "@/components/schedule-builder/OverviewCard";

const ScheduleBuilderOverview = () => {
  const steps = [
    {
      title: "Grupy harmonogramowe",
      description: "Twórz i zarządzaj grupami harmonogramowymi.",
      link: "/dashboard/schedule-builder/groups",
    },
    {
      title: "Wstępne harmonogramy",
      description: "Buduj wstępne harmonogramy na podstawie grup.",
      link: "/dashboard/schedule-builder/preliminary",
    },
    {
      title: "Finalne harmonogramy",
      description: "Generuj finalne harmonogramy na podstawie wstępnych.",
      link: "/dashboard/schedule-builder/final",
    },
  ];

  return (
    <div className="flex flex-col gap-4">
      <ScheduleBuilderNavigation />
      <h1 className="text-2xl font-bold">Budowanie harmonogramu</h1>
      <p className="text-sm text-gray-600">
        Witaj w module budowy harmonogramów. Wybierz etap procesu, aby rozpocząć
        pracę.
      </p>
      <div className="grid grid-cols-1 gap-4 sm:grid-cols-2 lg:grid-cols-3">
        {steps.map((step) => (
          <OverviewCard
            key={step.title}
            title={step.title}
            description={step.description}
            link={step.link}
          />
        ))}
      </div>
    </div>
  );
};

export default ScheduleBuilderOverview;
