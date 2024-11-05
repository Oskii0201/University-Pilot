import { FaCalendarCheck, FaChalkboardTeacher, FaUsers } from "react-icons/fa";

export const Features = () => {
  return (
    <section id="features" className="py-16 text-darkGray">
      <div className="mx-auto max-w-6xl px-4 md:px-6">
        <h2 className="mb-12 text-center text-3xl font-bold">
          Kluczowe Funkcje
        </h2>
        <div className="grid grid-cols-1 gap-8 sm:grid-cols-2 md:grid-cols-3">
          <div className="rounded-lg border-l-4 border-softGreen bg-white p-6 text-center shadow-lg">
            <FaCalendarCheck className="mx-auto mb-4 text-4xl text-blue-600" />
            <h3 className="mb-2 text-xl font-semibold">
              Automatyczne Harmonogramy
            </h3>
            <p>
              Generuj optymalne harmonogramy dla uczelni jednym kliknięciem.
            </p>
          </div>
          <div className="rounded-lg border-l-4 border-softRed bg-white p-6 text-center shadow-lg">
            <FaUsers className="mx-auto mb-4 text-4xl text-blue-600" />
            <h3 className="mb-2 text-xl font-semibold">Zarządzanie Grupami</h3>
            <p>Efektywne zarządzanie grupami studentów, salami i zasobami.</p>
          </div>
          <div className="rounded-lg border-l-4 border-mutedGold bg-white p-6 text-center shadow-lg">
            <FaChalkboardTeacher className="mx-auto mb-4 text-4xl text-blue-600" />
            <h3 className="mb-2 text-xl font-semibold">Wykładowcy i Zasoby</h3>
            <p>Organizuj harmonogramy i zasoby dla wykładowców bez stresu.</p>
          </div>
        </div>
      </div>
    </section>
  );
};
