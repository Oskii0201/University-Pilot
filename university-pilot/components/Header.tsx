const Header = () => {
  return (
    <header className="bg-lightGray text-darkGray flex items-center justify-between p-4 shadow">
      <h1 className="text-xl font-semibold">University Pilot</h1>
      <div>
        <span className="mr-4">
          Zalogowany jako: <strong>Administrator</strong>
        </span>
        <a href="/logout" className="text-blue-600 hover:underline">
          Wyloguj się
        </a>
      </div>
    </header>
  );
};

export default Header;
