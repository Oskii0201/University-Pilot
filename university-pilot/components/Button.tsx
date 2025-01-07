import React from "react";
import Link from "next/link";

type ButtonProps = {
  children: string;
  color?: "green" | "red" | "blue";
  onClick?: () => void;
  href?: string;
  width?: string;
  additionalClasses?: string;
  type?: "submit" | "reset" | "button" | undefined;
  disabled?: boolean;
  bold?: boolean;
};

export const Button: React.FC<ButtonProps> = ({
  children,
  color = "green",
  onClick,
  href,
  width = "w-full",
  additionalClasses = "",
  type,
  disabled = false,
  bold = true,
}) => {
  const baseStyles = `rounded-md px-4 py-2 transition-colors text-center ${width} ${bold ? `font-semibold` : ``} ${additionalClasses}`;
  const colorStyles = {
    green: "text-offWhite bg-mutedGreen hover:bg-softGreen",
    red: "text-offWhite bg-mutedRed hover:bg-softRed",
    blue: "text-offWhite bg-darkBlue hover:bg-softBlue",
    grey: "bg-gray-200 text-gray-600 hover:bg-blue-500 hover:text-offWhite",
  };
  const disabledStyles = "opacity-50 cursor-not-allowed";

  if (href) {
    return (
      <Link
        href={href}
        className={`${baseStyles} ${colorStyles[color]} ${disabled ? disabledStyles : colorStyles[color]}`}
        aria-disabled={disabled}
      >
        {children}
      </Link>
    );
  }

  return (
    <button
      type={type}
      onClick={!disabled ? onClick : undefined}
      className={`${baseStyles} ${colorStyles[color]} ${disabled ? disabledStyles : colorStyles[color]}`}
      disabled={disabled}
    >
      {children}
    </button>
  );
};
