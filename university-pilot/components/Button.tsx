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
}) => {
  const baseStyles = `rounded-md px-6 py-3 font-semibold text-offWhite transition-colors text-center ${width} ${additionalClasses}`;
  const colorStyles = {
    green: "bg-mutedGreen hover:bg-softGreen",
    red: "bg-mutedRed hover:bg-softRed",
    blue: "bg-darkBlue hover:bg-softBlue",
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
