"use client";
import { useRouter } from "next/navigation";
import { login } from "@/app/lib/auth";
import { toast } from "react-toastify";
import React, { useState } from "react";
import { FaLock, FaUser } from "react-icons/fa";

const LoginForm = () => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [showPassword, setShowPassword] = useState(false);
  const router = useRouter();
  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await login(email, password);
      router.push("/dashboard");
    } catch (e) {
      toast.error(
        <div>
          <h2 className="font-semibold"> Oops!</h2>
          <p>{e.message}</p>
        </div>,
      );
    }
  };
  return (
    <form onSubmit={handleSubmit} className="flex flex-col space-y-4">
      <div className="relative">
        <FaUser className="absolute left-3 top-1/3 text-gray-400" />
        <input
          type="email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          placeholder="Email"
          className="w-full rounded-md bg-gray-800 px-10 py-3 text-offWhite placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-green-600"
          required
        />
      </div>

      <div className="relative">
        <FaLock className="absolute left-3 top-1/3 text-gray-400" />
        <input
          type={showPassword ? "text" : "password"}
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          placeholder="Hasło"
          className="w-full rounded-md bg-gray-800 px-10 py-3 text-offWhite placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-green-600"
          required
        />
      </div>

      <div className="flex items-center justify-between text-gray-400">
        <label className="flex items-center space-x-2">
          <input
            type="checkbox"
            checked={showPassword}
            onChange={() => setShowPassword(!showPassword)}
            className="form-checkbox h-4 w-4 text-green-500"
          />
          <span className="text-sm">Pokaż hasło</span>
        </label>
        <a href="#" className="text-sm hover:underline">
          Zapomniałeś hasła?
        </a>
      </div>

      <button
        type="submit"
        className="mt-6 w-full rounded-md bg-green-700 py-3 font-semibold uppercase text-offWhite transition-colors hover:bg-green-600"
      >
        Login
      </button>
    </form>
  );
};

export default LoginForm;
