import React from "react";

type LoadingCircleProps = {
  isOverlay?: boolean;
};

export const LoadingCircle: React.FC<LoadingCircleProps> = ({ isOverlay }) => {
  return (
    <div
      className={`${
        isOverlay
          ? "absolute inset-0 z-50 bg-gray-100 bg-opacity-50"
          : "relative"
      } flex items-center justify-center`}
    >
      <div className="h-10 w-10 animate-spin rounded-full border-4 border-solid border-blue-500 border-t-transparent"></div>
    </div>
  );
};
