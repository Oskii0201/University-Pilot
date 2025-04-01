import Image from "next/image";
import universityPilotDashboard from "/public/imageplacehgolder.png";
import { Button } from "@/components/ui/Button";

export const Hero = () => {
  return (
    <section
      id="hero"
      className="flex flex-col items-center justify-center gap-10 rounded-bl-[5rem] bg-gradient-to-br from-darkNavy via-gray-700 to-gray-800 py-16 md:rounded-bl-[15rem] md:py-20"
    >
      <h1 className="px-6 text-center text-3xl font-bold md:text-5xl">
        Automatyzacja planowania zajęć na twojej uczelni
      </h1>
      <div className="flex flex-col items-center justify-center gap-10 md:flex-row">
        <div className="flex flex-col justify-center gap-8 md:w-1/2">
          <p className="px-6 text-center text-lg md:max-w-md md:px-0 md:text-left md:text-xl">
            University Pilot to nowoczesna aplikacja webowa, która automatyzuje
            proces tworzenia harmonogramów zajęć na uczelniach wyższych.
            Oszczędzaj czas i eliminuj błędy podczas planowania.
          </p>
          <Button
            color="green"
            href="/#howitworks"
            width="w-fit"
            additionalClasses="mx-auto uppercase"
          >
            Dowiedz się więcej
          </Button>
        </div>
        <div className="mt-8 w-full md:w-auto">
          <Image
            src={universityPilotDashboard}
            alt="University Pilot dashboard"
            className="mx-auto h-auto w-full max-w-xs md:w-96"
          />
        </div>
      </div>
    </section>
  );
};
