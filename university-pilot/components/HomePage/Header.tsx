import Link from "next/link";

export const Header = () => {
  return (
    <header className="flex flex-col items-center justify-between gap-4 bg-darkNavy p-5 md:flex-row md:px-10">
      <h1 className="text-2xl font-bold md:text-3xl">University Pilot</h1>
      <Link
        href="/login"
        className="text-lg font-semibold text-gray-100 transition hover:text-green-600 hover:underline md:text-xl"
      >
        Zaloguj się
      </Link>
    </header>
  );
};
