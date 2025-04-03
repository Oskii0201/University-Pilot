import React from "react";

type LoadingCircleProps = {
  isOverlay?: boolean;
  message?: string;
};

export const LoadingCircle: React.FC<LoadingCircleProps> = ({
  isOverlay,
  message,
}) => {
  return (
    <div
      className={`${
        isOverlay
          ? "absolute inset-0 z-50 flex flex-col bg-gray-100 bg-opacity-50"
          : "relative flex flex-col"
      } items-center justify-center`}
    >
      <div className="h-10 w-10 animate-spin rounded-full border-4 border-solid border-blue-500 border-t-transparent"></div>
      {message && <p className="mt-2 text-gray-800">{message}</p>}{" "}
    </div>
  );
};
