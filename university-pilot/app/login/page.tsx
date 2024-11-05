import React from "react";
import { FaUser } from "react-icons/fa";
import LoginForm from "@/components/LoginForm";

export default function LoginPage() {
  return (
    <div className="flex w-full items-center justify-center bg-gradient-to-tl from-darkNavy via-gray-700 to-gray-800">
      <div className="rounded-xl bg-white/10 bg-opacity-20 p-8 shadow-lg backdrop-blur-md md:w-96">
        <div className="mb-6 flex justify-center">
          <div className="flex items-center justify-center rounded-full bg-green-700 p-4">
            <FaUser className="text-4xl text-offWhite" />
          </div>
        </div>

        <h2 className="mb-6 text-center text-2xl font-semibold text-offWhite">
          Zaloguj się
        </h2>
        <LoginForm />
      </div>
    </div>
  );
}
