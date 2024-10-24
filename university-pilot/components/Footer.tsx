import Link from "next/link";

const Footer = () => {
  return (
    <footer className="bg-darkNavy text-offWhite py-4 text-center">
      <p>
        &copy; 2024{" "}
        <Link
          href="https://github.com/Oskii0201/University-Pilot"
          target="_blank"
          rel="noopener noreferrer"
          className="text-sky-500 hover:underline"
        >
          University Pilot
        </Link>
        . All rights Reserved.
      </p>
    </footer>
  );
};

export default Footer;
