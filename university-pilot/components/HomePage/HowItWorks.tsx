export const HowItWorks = () => {
  return (
    <section id="howitworks" className="py-16 text-darkGray">
      <div className="mx-auto max-w-6xl px-4 text-center md:px-6">
        <h2 className="mb-12 text-3xl font-bold">Jak To Działa?</h2>
        <div className="flex flex-col items-center gap-8 md:flex-row">
          <div className="w-full rounded-lg border-r-4 border-softGreen bg-white p-6 shadow-lg md:w-1/3">
            <h3 className="mb-2 text-xl font-semibold">Krok 1: Rejestracja</h3>
            <p>Załóż konto i skonfiguruj ustawienia uczelni.</p>
          </div>
          <div className="w-full rounded-lg border-r-4 border-softRed bg-white p-6 shadow-lg md:w-1/3">
            <h3 className="mb-2 text-xl font-semibold">Krok 2: Dodaj Zasoby</h3>
            <p>Wprowadź grupy, wykładowców i sale.</p>
          </div>
          <div className="w-full rounded-lg border-r-4 border-mutedGold bg-white p-6 shadow-lg md:w-1/3">
            <h3 className="mb-2 text-xl font-semibold">
              Krok 3: Generuj Harmonogram
            </h3>
            <p>Automatycznie wygeneruj harmonogram zajęć.</p>
          </div>
        </div>
      </div>
    </section>
  );
};
