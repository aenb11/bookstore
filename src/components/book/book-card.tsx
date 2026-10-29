import Link from "next/link";
import { discountPercent, formatCompact, formatVND } from "@/lib/format";
import type { Book } from "@/lib/types";
import { cn } from "@/lib/utils";
import { BookCover } from "./book-cover";
import { RatingStars } from "./rating-stars";

/** Compact marketplace-style product tile (Fahasa/Tiki density). */
export function BookCard({ book, className }: { book: Book; className?: string }) {
  const discount = discountPercent(book.price, book.originalPrice);

  return (
    <Link
      href={`/books/${book.slug}`}
      className={cn(
        "group flex flex-col gap-2 rounded-lg bg-card p-3 transition-shadow hover:shadow-[0_2px_12px_rgb(0_0_0/0.12)]",
        className,
      )}
    >
      <div className="px-[10%] pt-1">
        <BookCover book={book} />
      </div>
      <h3 className="line-clamp-2 min-h-10 text-sm leading-5 font-normal group-hover:text-primary">{book.title}</h3>
      <div className="mt-auto space-y-1">
        <div className="flex items-center gap-1.5">
          <span className="font-bold text-primary">{formatVND(book.price)}</span>
          {discount > 0 && (
            <span className="rounded-sm bg-primary px-1 text-xs font-semibold text-primary-foreground">-{discount}%</span>
          )}
        </div>
        {book.originalPrice && (
          <p className="text-xs text-muted-foreground line-through">
            <span className="sr-only">Giá gốc </span>
            {formatVND(book.originalPrice)}
          </p>
        )}
        <div className="flex items-center gap-2 text-xs text-muted-foreground">
          <RatingStars rating={book.rating} />
          <span>Đã bán {formatCompact(book.soldCount)}</span>
        </div>
      </div>
    </Link>
  );
}

export function BookGrid({ books, className }: { books: Book[]; className?: string }) {
  return (
    <div className={cn("grid grid-cols-2 gap-2 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5 xl:grid-cols-6", className)}>
      {books.map((book) => (
        <BookCard key={book.id} book={book} />
      ))}
    </div>
  );
}
