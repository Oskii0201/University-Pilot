export const Benefits = () => {
  return (
    <section id="benefits" className="bg-gray-50 py-16 text-darkGray">
      <div className="mx-auto max-w-6xl px-4 text-center md:px-6">
        <h2 className="mb-12 text-3xl font-bold">Dlaczego Warto?</h2>
        <div className="grid grid-cols-1 gap-8 md:grid-cols-3">
          <div className="rounded-lg p-6 shadow-lg">
            <h3 className="mb-2 text-xl font-semibold">Oszczędność Czasu</h3>
            <p>
              Automatyzacja procesów pozwala zaoszczędzić czas na organizację
              zajęć.
            </p>
          </div>
          <div className="rounded-lg p-6 shadow-lg">
            <h3 className="mb-2 text-xl font-semibold">Lepsza Organizacja</h3>
            <p>
              Intuicyjny system, który pomaga w zarządzaniu zasobami uczelni.
            </p>
          </div>
          <div className="rounded-lg p-6 shadow-lg">
            <h3 className="mb-2 text-xl font-semibold">Efektywność</h3>
            <p>
              Optymalne harmonogramy poprawiają efektywność wykładowców i
              studentów.
            </p>
          </div>
        </div>
      </div>
    </section>
  );
};
