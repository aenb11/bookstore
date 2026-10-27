import { cn } from "@/lib/utils";
import type { Book } from "@/lib/types";

// [background, shape, ink] — muted tones so covers don't fight with the UI.
const palettes = [
  ["oklch(0.36 0.08 265)", "oklch(0.45 0.09 265)", "oklch(0.97 0.01 265)"],
  ["oklch(0.9 0.03 80)", "oklch(0.84 0.05 75)", "oklch(0.3 0.03 60)"],
  ["oklch(0.42 0.06 160)", "oklch(0.5 0.07 160)", "oklch(0.97 0.01 160)"],
  ["oklch(0.93 0.015 265)", "oklch(0.87 0.03 265)", "oklch(0.3 0.05 265)"],
  ["oklch(0.55 0.13 25)", "oklch(0.62 0.12 30)", "oklch(0.98 0.01 60)"],
  ["oklch(0.25 0.02 265)", "oklch(0.32 0.03 265)", "oklch(0.92 0.03 85)"],
  ["oklch(0.85 0.05 230)", "oklch(0.78 0.06 230)", "oklch(0.28 0.05 250)"],
  ["oklch(0.88 0.04 20)", "oklch(0.82 0.06 20)", "oklch(0.32 0.06 20)"],
] as const;

function hash(text: string) {
  let value = 0;
  for (const char of text) value = (value * 31 + char.charCodeAt(0)) >>> 0;
  return value;
}

type BookCoverProps = {
  book: Pick<Book, "id" | "title" | "author">;
  /** `xs` drops the text entirely for thumbnail-sized slots. */
  size?: "xs" | "sm" | "md" | "lg";
  className?: string;
};

/** Generated typographic cover, so the mockup needs no external images. */
export function BookCover({ book, size = "md", className }: BookCoverProps) {
  const seed = hash(book.id + book.title);
  const [bg, shape, ink] = palettes[seed % palettes.length];
  const pattern = seed % 3;

  return (
    <div
      role="img"
      aria-label={`Bìa sách ${book.title} — ${book.author}`}
      className={cn(
        "relative aspect-[2/3] w-full overflow-hidden rounded-md shadow-[0_1px_2px_rgb(0_0_0/0.12),0_10px_24px_-14px_rgb(0_0_0/0.4)]",
        (size === "sm" || size === "xs") && "rounded-sm",
        className,
      )}
      style={{ background: bg, color: ink }}
    >
      <svg aria-hidden viewBox="0 0 100 150" className="absolute inset-0 size-full" preserveAspectRatio="none">
        {pattern === 0 && <circle cx="80" cy="125" r="42" fill={shape} />}
        {pattern === 1 && <rect x="0" y="100" width="100" height="50" fill={shape} />}
        {pattern === 2 && <line x1="12" y1="112" x2="88" y2="112" stroke={ink} strokeOpacity="0.4" strokeWidth="0.6" />}
      </svg>
      <div
        className={cn(
          "relative flex h-full flex-col justify-between p-[11%]",
          size === "sm" && "p-[9%]",
          size === "xs" && "hidden",
        )}
      >
        <p
          className={cn(
            "font-heading leading-tight font-bold text-balance",
            size === "sm" && "text-[0.6rem]",
            size === "md" && "text-base sm:text-lg",
            size === "lg" && "text-3xl sm:text-4xl",
          )}
        >
          {book.title}
        </p>
        <p
          className={cn(
            "tracking-wide uppercase opacity-75",
            size === "sm" && "hidden",
            size === "md" && "text-[0.6rem]",
            size === "lg" && "text-xs",
          )}
        >
          {book.author}
        </p>
      </div>
    </div>
  );
}
