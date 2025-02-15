import Link from "next/link";
import React from "react";

interface OverviewCardProps {
  title: string;
  description: string;
  link: string;
}

const OverviewCard: React.FC<OverviewCardProps> = ({
  title,
  description,
  link,
}) => {
  return (
    <div className="flex flex-col gap-2 rounded-lg border p-4 shadow transition hover:shadow-lg">
      <h2 className="text-lg font-semibold">{title}</h2>
      <p className="text-sm text-gray-600">{description}</p>
      <Link href={link} className="mt-auto text-blue-500 hover:underline">
        Przejdź do {title.toLowerCase()}
      </Link>
    </div>
  );
};

export default OverviewCard;
