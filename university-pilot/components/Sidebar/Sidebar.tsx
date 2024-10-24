import Navigation from "@/components/Sidebar/Navigation";

const Sidebar = () => {
  return (
    <aside className="bg-darkNavy text-offWhite fixed flex min-h-screen w-24 flex-col gap-8 md:w-48 md:p-6 lg:w-72 lg:p-10">
      <div className="text-2xl text-sky-400">[Logo]</div>
      <Navigation />
    </aside>
  );
};

export default Sidebar;
