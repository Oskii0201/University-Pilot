import { render, screen, fireEvent } from "@testing-library/react";
import "@testing-library/jest-dom";
import { Button } from "@/src/components/ui/Button";

describe("Button component", () => {
  it("renders a button with default props", () => {
    render(<Button>Click Me</Button>);

    const button = screen.getByRole("button", { name: /click me/i });
    expect(button).toBeInTheDocument();
    expect(button).toHaveClass(
      "rounded-md px-4 py-2 transition-colors text-center w-full font-semibold",
    );
    expect(button).toHaveClass(
      "text-offWhite bg-mutedGreen hover:bg-softGreen",
    );
  });

  it("renders a button with a custom color", () => {
    render(<Button color="red">Click Me</Button>);

    const button = screen.getByRole("button", { name: /click me/i });
    expect(button).toHaveClass("bg-mutedRed hover:bg-softRed");
  });

  it("renders a button as a link when href is provided", () => {
    render(<Button href="/about">About</Button>);

    const link = screen.getByRole("link", { name: /about/i });
    expect(link).toBeInTheDocument();
    expect(link).toHaveAttribute("href", "/about");
  });

  it("applies additional classes", () => {
    render(<Button additionalClasses="custom-class">Click Me</Button>);

    const button = screen.getByRole("button", { name: /click me/i });
    expect(button).toHaveClass("custom-class");
  });

  it("is disabled and does not call onClick when disabled", () => {
    const handleClick = jest.fn();
    render(
      <Button onClick={handleClick} disabled>
        Disabled
      </Button>,
    );

    const button = screen.getByRole("button", { name: /disabled/i });
    expect(button).toBeDisabled();
    fireEvent.click(button);
    expect(handleClick).not.toHaveBeenCalled();
  });

  it("calls onClick when clicked", () => {
    const handleClick = jest.fn();
    render(<Button onClick={handleClick}>Click Me</Button>);

    const button = screen.getByRole("button", { name: /click me/i });
    fireEvent.click(button);
    expect(handleClick).toHaveBeenCalledTimes(1);
  });

  it("renders with a custom width", () => {
    render(<Button width="w-1/2">Custom Width</Button>);

    const button = screen.getByRole("button", { name: /custom width/i });
    expect(button).toHaveClass("w-1/2");
  });

  it("renders bold text by default", () => {
    render(<Button>Bold Text</Button>);

    const button = screen.getByRole("button", { name: /bold text/i });
    expect(button).toHaveClass("font-semibold");
  });

  it("does not render bold text when bold is false", () => {
    render(<Button bold={false}>Not Bold</Button>);

    const button = screen.getByRole("button", { name: /not bold/i });
    expect(button).not.toHaveClass("font-semibold");
  });
});
