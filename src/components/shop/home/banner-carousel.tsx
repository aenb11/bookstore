"use client";

import Link from "next/link";
import { useEffect, useState } from "react";
import { ChevronLeft, ChevronRight } from "lucide-react";
import { BookCover } from "@/components/book/book-cover";
import type { Book } from "@/lib/types";
import { cn } from "@/lib/utils";

export type Banner = {
  title: string;
  subtitle: string;
  href: string;
  className: string;
  books: Book[];
};

export function BannerCarousel({ banners }: { banners: Banner[] }) {
  const [index, setIndex] = useState(0);
  const [paused, setPaused] = useState(false);

  useEffect(() => {
    if (paused) return;
    const timer = setInterval(() => setIndex((i) => (i + 1) % banners.length), 5000);
    return () => clearInterval(timer);
  }, [paused, banners.length]);

  const go = (step: number) => setIndex((i) => (i + step + banners.length) % banners.length);

  return (
    <div
      className="group relative h-full min-h-56 overflow-hidden rounded-lg"
      onMouseEnter={() => setPaused(true)}
      onMouseLeave={() => setPaused(false)}
      aria-roledescription="carousel"
    >
      {banners.map((banner, i) => (
        <Link
          key={banner.title}
          href={banner.href}
          aria-hidden={i !== index}
          tabIndex={i === index ? 0 : -1}
          className={cn(
            "absolute inset-0 flex items-center justify-between gap-6 px-8 transition-opacity duration-500 sm:px-12",
            banner.className,
            i === index ? "opacity-100" : "pointer-events-none opacity-0",
          )}
        >
          <div className="max-w-xs space-y-2">
            <p className="text-2xl leading-tight font-black sm:text-3xl">{banner.title}</p>
            <p className="text-sm opacity-85">{banner.subtitle}</p>
            <span className="mt-2 inline-block rounded-md bg-card px-4 py-2 text-sm font-semibold text-foreground">
              Mua ngay
            </span>
          </div>
          <div className="hidden w-64 shrink-0 items-end gap-3 sm:flex">
            {banner.books.map((book, j) => (
              <div key={book.id} className={cn("flex-1", j === 1 && "-translate-y-3")}>
                <BookCover book={book} size="sm" />
              </div>
            ))}
          </div>
        </Link>
      ))}

      <button
        type="button"
        aria-label="Banner trước"
        onClick={() => go(-1)}
        className="absolute top-1/2 left-2 grid size-8 -translate-y-1/2 place-items-center rounded-full bg-card/90 opacity-0 shadow transition-opacity group-hover:opacity-100 focus-visible:opacity-100"
      >
        <ChevronLeft className="size-4" />
      </button>
      <button
        type="button"
        aria-label="Banner tiếp theo"
        onClick={() => go(1)}
        className="absolute top-1/2 right-2 grid size-8 -translate-y-1/2 place-items-center rounded-full bg-card/90 opacity-0 shadow transition-opacity group-hover:opacity-100 focus-visible:opacity-100"
      >
        <ChevronRight className="size-4" />
      </button>
      <div className="absolute bottom-3 left-1/2 flex -translate-x-1/2 gap-1.5">
        {banners.map((banner, i) => (
          <button
            key={banner.title}
            type="button"
            aria-label={`Banner ${i + 1}`}
            aria-current={i === index}
            onClick={() => setIndex(i)}
            className={cn("h-1.5 rounded-full bg-card transition-all", i === index ? "w-5" : "w-1.5 opacity-60")}
          />
        ))}
      </div>
    </div>
  );
}
