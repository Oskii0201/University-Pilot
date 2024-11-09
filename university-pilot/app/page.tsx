import { Header } from "@/components/HomePage/Header";
import { Hero } from "@/components/HomePage/Hero";
import { Features } from "@/components/HomePage/Features";
import { Benefits } from "@/components/HomePage/Benefits";
import { HowItWorks } from "@/components/HomePage/HowItWorks";
import { CTO } from "@/components/HomePage/CTO";

export default function Home() {
  return (
    <div className="text-off w-full text-offWhite">
      <Header />
      <Hero />
      <Features />
      <Benefits />
      <HowItWorks />
      <CTO />
    </div>
  );
}
