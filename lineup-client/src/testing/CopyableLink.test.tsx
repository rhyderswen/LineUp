/**
 * @vitest-environment jsdom
 */
import { describe, it, expect, vi, beforeEach, afterEach } from "vitest";
import { render, screen, fireEvent, act } from "@testing-library/react";
import CopyableLink from "../components/CopyableLink";
import "@testing-library/jest-dom/vitest";

describe("CopyableLink component", () => {
  const testUrl = "https://example.com";

  beforeEach(() => {
    vi.useFakeTimers();
    Object.assign(navigator, {
      clipboard: {
        writeText: vi.fn(),
      },
    });
  });

  afterEach(() => {
    vi.clearAllTimers();
    vi.restoreAllMocks();
  });

  it("renders the URL initially", () => {
    render(<CopyableLink url={testUrl} />);
    const button = screen.getByRole("button", { name: testUrl });
    expect(button).toBeInTheDocument();
    expect(button).toHaveTextContent(testUrl);
  });

  it("copies the URL to clipboard and shows 'Copied'", async () => {
    const button = screen.getByRole("button");

    await act(async () => {
      fireEvent.click(button);
    });

    expect(navigator.clipboard.writeText).toHaveBeenCalledWith(testUrl);
    expect(button).toHaveTextContent("Copied");
  });

  it("resets the text back to the URL after 1.5 seconds", async () => {
    const button = screen.getByRole("button");

    await act(async () => {
      fireEvent.click(button);
    });

    expect(button).toHaveTextContent("Copied");

    act(() => {
      vi.advanceTimersByTime(1500);
    });

    expect(button).toHaveTextContent(testUrl);
  });
});
