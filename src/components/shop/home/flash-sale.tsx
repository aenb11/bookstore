"use client";

import Link from "next/link";
import { useSyncExternalStore } from "react";
import { ChevronRight, Zap } from "lucide-react";
import { BookCover } from "@/components/book/book-cover";
import { discountPercent, formatVND } from "@/lib/format";
import type { Book } from "@/lib/types";

function subscribe(onTick: () => void) {
  const timer = setInterval(onTick, 1000);
  return () => clearInterval(timer);
}

/** Seconds left until midnight; null on the server so the countdown never mismatches on hydration. */
function useSecondsToMidnight() {
  return useSyncExternalStore(
    subscribe,
    () => {
      const now = new Date();
      const midnight = new Date(now);
      midnight.setHours(24, 0, 0, 0);
      return Math.floor((midnight.getTime() - now.getTime()) / 1000);
    },
    () => null,
  );
}

function Countdown() {
  const seconds = useSecondsToMidnight();
  const parts =
    seconds === null
      ? ["--", "--", "--"]
      : [seconds / 3600, (seconds % 3600) / 60, seconds % 60].map((n) => String(Math.floor(n)).padStart(2, "0"));

  return (
    <span className="flex items-center gap-1" aria-label="Thời gian còn lại">
      {parts.map((part, i) => (
        <span key={i} className="flex items-center gap-1">
          <span className="rounded bg-foreground px-1.5 py-0.5 text-sm font-bold text-background tabular-nums">
            {part}
          </span>
          {i < 2 && <span className="font-bold">:</span>}
        </span>
      ))}
    </span>
  );
}

export function FlashSale({ books }: { books: Book[] }) {
  return (
    <section aria-labelledby="flash-sale-heading" className="overflow-hidden rounded-lg bg-card">
      <div className="flex flex-wrap items-center gap-4 border-b px-4 py-3">
        <h2 id="flash-sale-heading" className="flex items-center gap-1.5 text-xl font-black text-primary uppercase italic">
          <Zap className="size-5 fill-current" /> Flash Sale
        </h2>
        <span className="text-sm text-muted-foreground">Kết thúc trong</span>
        <Countdown />
        <Link href="/books?sale=1" className="ml-auto flex items-center text-sm font-medium text-primary">
          Xem tất cả <ChevronRight className="size-4" />
        </Link>
      </div>
      <ul className="grid grid-flow-col auto-cols-[45%] gap-2 overflow-x-auto p-3 sm:auto-cols-[30%] md:grid-flow-row md:grid-cols-6 md:overflow-visible">
        {books.map((book, i) => {
          const sold = 30 + ((i * 17) % 60);
          return (
            <li key={book.id}>
              <Link href={`/books/${book.slug}`} className="group flex h-full flex-col gap-2 rounded-md p-2 hover:bg-muted">
                <div className="relative px-[10%]">
                  <BookCover book={book} />
                  <span className="absolute top-0 right-0 rounded-bl-md bg-primary px-1.5 py-0.5 text-xs font-bold text-primary-foreground">
                    -{discountPercent(book.price, book.originalPrice)}%
                  </span>
                </div>
                <p className="line-clamp-2 min-h-10 text-sm leading-5 group-hover:text-primary">{book.title}</p>
                <div className="mt-auto">
                  <p className="font-bold text-primary">{formatVND(book.price)}</p>
                  <p className="text-xs text-muted-foreground line-through">{formatVND(book.originalPrice!)}</p>
                </div>
                <div className="relative h-4 overflow-hidden rounded-full bg-[oklch(0.72_0.12_27)]">
                  <div className="h-full rounded-full bg-primary" style={{ width: `${sold}%` }} />
                  <span className="absolute inset-0 grid place-items-center text-[0.65rem] font-semibold text-white">
                    Đã bán {sold}
                  </span>
                </div>
              </Link>
            </li>
          );
        })}
      </ul>
    </section>
  );
}
