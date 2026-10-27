"use client";

import Link from "next/link";
import { useState } from "react";
import { BookCover } from "@/components/book/book-cover";
import { RatingStars } from "@/components/book/rating-stars";
import { formatCompact, formatVND } from "@/lib/format";
import type { Book, Category } from "@/lib/types";
import { cn } from "@/lib/utils";

export function Ranking({ groups }: { groups: { category: Category; books: Book[] }[] }) {
  const [tab, setTab] = useState(0);
  const [active, setActive] = useState(0);
  const books = groups[tab].books;
  const selected = books[Math.min(active, books.length - 1)];

  return (
    <section aria-labelledby="ranking-heading" className="rounded-lg bg-card">
      <div className="flex flex-wrap items-center gap-x-6 gap-y-2 border-b px-4 pt-3">
        <h2 id="ranking-heading" className="pb-3 text-lg font-bold">
          Bảng xếp hạng bán chạy tuần
        </h2>
        <div role="tablist" className="flex gap-1 overflow-x-auto">
          {groups.map((group, i) => (
            <button
              key={group.category.id}
              role="tab"
              type="button"
              aria-selected={i === tab}
              onClick={() => {
                setTab(i);
                setActive(0);
              }}
              className={cn(
                "border-b-2 px-3 pb-3 text-sm whitespace-nowrap transition-colors",
                i === tab ? "border-primary font-semibold text-primary" : "border-transparent text-muted-foreground hover:text-foreground",
              )}
            >
              {group.category.name}
            </button>
          ))}
        </div>
      </div>

      <div className="grid gap-4 p-4 md:grid-cols-[minmax(0,1fr)_minmax(0,1fr)]">
        <ol className="divide-y">
          {books.map((book, i) => (
            <li key={book.id}>
              <Link
                href={`/books/${book.slug}`}
                onMouseEnter={() => setActive(i)}
                onFocus={() => setActive(i)}
                className={cn(
                  "flex items-center gap-3 rounded-md px-2 py-2.5 transition-colors",
                  i === active && "bg-muted",
                )}
              >
                <span
                  className={cn(
                    "w-6 shrink-0 text-center text-lg font-black tabular-nums",
                    i < 3 ? "text-primary" : "text-muted-foreground",
                  )}
                >
                  {String(i + 1).padStart(2, "0")}
                </span>
                <div className="w-10 shrink-0">
                  <BookCover book={book} size="xs" />
                </div>
                <div className="min-w-0 flex-1">
                  <p className="truncate text-sm font-medium">{book.title}</p>
                  <p className="truncate text-xs text-muted-foreground">{book.author}</p>
                </div>
                <span className="text-xs text-muted-foreground">{formatCompact(book.soldCount)} đã bán</span>
              </Link>
            </li>
          ))}
        </ol>

        {selected && (
          <div className="hidden gap-5 rounded-md border p-5 md:flex">
            <div className="w-36 shrink-0">
              <BookCover book={selected} />
            </div>
            <div className="min-w-0 space-y-2">
              <p className="text-lg leading-snug font-bold">{selected.title}</p>
              <p className="text-sm text-muted-foreground">
                Tác giả: <span className="text-foreground">{selected.author}</span>
              </p>
              <p className="text-sm text-muted-foreground">
                NXB: <span className="text-foreground">{selected.publisher}</span>
              </p>
              <RatingStars rating={selected.rating} count={selected.reviewCount} />
              <p className="text-xl font-bold text-primary">{formatVND(selected.price)}</p>
              <p className="line-clamp-4 text-sm text-muted-foreground">{selected.description}</p>
            </div>
          </div>
        )}
      </div>
    </section>
  );
}
