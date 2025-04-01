import { Button } from "@/components/ui/Button";

export const CTO = () => {
  return (
    <section
      id="cto"
      className="flex flex-col items-center border-b border-gray-500 bg-darkNavy px-6 py-12 md:px-8 md:py-16"
    >
      <h2 className="mb-6 text-3xl font-bold">Gotowy, aby zacząć?</h2>
      <p className="mb-8 text-lg">
        Skontaktuj się z nami, a umówimy się na prezentację i otrzymasz darmowy
        okres próbny.
      </p>
      <form className="w-full max-w-md">
        <input
          type="text"
          name="name"
          placeholder="Twoje Imię"
          className="mb-4 w-full rounded bg-gray-800 px-4 py-2"
          required
        />
        <input
          type="email"
          name="email"
          placeholder="Twój Email"
          className="mb-4 w-full rounded bg-gray-800 px-4 py-2"
          required
        />
        <textarea
          name="message"
          placeholder="Wiadomość"
          className="mb-4 h-32 w-full rounded bg-gray-800 px-4 py-2"
          required
        ></textarea>
        <Button color="green" width="w-fit" additionalClasses="uppercase">
          Wyślij
        </Button>
      </form>
    </section>
  );
};
