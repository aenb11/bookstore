"use client";

import Link from "next/link";
import { Minus, Plus, ShoppingBag, Trash2 } from "lucide-react";
import { BookCover } from "@/components/book/book-cover";
import { Button } from "@/components/ui/button";
import { Separator } from "@/components/ui/separator";
import {
  Sheet,
  SheetClose,
  SheetContent,
  SheetDescription,
  SheetFooter,
  SheetHeader,
  SheetTitle,
  SheetTrigger,
} from "@/components/ui/sheet";
import { cart, useCart } from "@/lib/cart-store";
import { formatVND } from "@/lib/format";
import { FREE_SHIPPING_THRESHOLD, getBookById } from "@/lib/mock-data";

export function useCartLines() {
  const items = useCart();
  const lines = items.flatMap((item) => {
    const book = getBookById(item.bookId);
    return book ? [{ ...item, book }] : [];
  });
  const subtotal = lines.reduce((sum, line) => sum + line.book.price * line.quantity, 0);
  const count = lines.reduce((sum, line) => sum + line.quantity, 0);
  return { lines, subtotal, count };
}

export function QuantityStepper({ bookId, quantity, max }: { bookId: string; quantity: number; max: number }) {
  return (
    <div className="inline-flex items-center rounded-md border">
      <Button
        variant="ghost"
        size="icon-sm"
        aria-label="Giảm số lượng"
        onClick={() => cart.setQuantity(bookId, quantity - 1)}
      >
        <Minus />
      </Button>
      <span className="w-8 text-center text-sm tabular-nums" aria-live="polite">
        {quantity}
      </span>
      <Button
        variant="ghost"
        size="icon-sm"
        aria-label="Tăng số lượng"
        disabled={quantity >= max}
        onClick={() => cart.setQuantity(bookId, quantity + 1)}
      >
        <Plus />
      </Button>
    </div>
  );
}

export function CartSheet() {
  const { lines, subtotal, count } = useCartLines();
  const remaining = FREE_SHIPPING_THRESHOLD - subtotal;

  return (
    <Sheet>
      <SheetTrigger asChild>
        <Button
          variant="ghost"
          className="relative h-12 flex-col gap-0.5 px-2 text-xs font-normal text-muted-foreground"
          aria-label={`Giỏ hàng, ${count} sản phẩm`}
        >
          <ShoppingBag className="size-5" />
          <span className="hidden sm:inline">Giỏ hàng</span>
          {count > 0 && (
            <span className="absolute top-0.5 right-1 grid min-w-4.5 place-items-center rounded-full bg-primary px-1 text-[0.65rem] leading-4.5 font-semibold text-primary-foreground">
              {count}
            </span>
          )}
        </Button>
      </SheetTrigger>
      <SheetContent className="w-full sm:max-w-md">
        <SheetHeader>
          <SheetTitle className="font-heading text-xl">Giỏ hàng của bạn</SheetTitle>
          <SheetDescription>
            {count > 0 ? `${count} sản phẩm` : "Chưa có sản phẩm nào"}
          </SheetDescription>
        </SheetHeader>

        {lines.length === 0 ? (
          <div className="flex flex-1 flex-col items-center justify-center gap-4 px-6 text-center">
            <ShoppingBag className="size-12 text-muted-foreground/50" />
            <p className="text-muted-foreground">Giỏ hàng đang trống. Khám phá vài cuốn sách hay nhé!</p>
            <SheetClose asChild>
              <Button asChild>
                <Link href="/books">Khám phá sách</Link>
              </Button>
            </SheetClose>
          </div>
        ) : (
          <>
            <div className="px-4">
              <p className="rounded-md bg-secondary px-3 py-2 text-sm text-secondary-foreground">
                {remaining > 0
                  ? `Mua thêm ${formatVND(remaining)} để được miễn phí vận chuyển.`
                  : "Đơn hàng của bạn được miễn phí vận chuyển"}
              </p>
            </div>
            <ul className="flex-1 space-y-4 overflow-y-auto px-4">
              {lines.map(({ book, quantity }) => (
                <li key={book.id} className="flex gap-3">
                  <div className="w-16 shrink-0">
                    <BookCover book={book} size="sm" />
                  </div>
                  <div className="flex min-w-0 flex-1 flex-col gap-1">
                    <SheetClose asChild>
                      <Link href={`/books/${book.slug}`} className="line-clamp-2 font-heading font-semibold hover:underline">
                        {book.title}
                      </Link>
                    </SheetClose>
                    <p className="text-xs text-muted-foreground">{book.author}</p>
                    <div className="mt-auto flex items-center justify-between">
                      <QuantityStepper bookId={book.id} quantity={quantity} max={book.stock} />
                      <span className="text-sm font-semibold">{formatVND(book.price * quantity)}</span>
                    </div>
                  </div>
                  <Button
                    variant="ghost"
                    size="icon-sm"
                    aria-label={`Xóa ${book.title}`}
                    onClick={() => cart.remove(book.id)}
                  >
                    <Trash2 />
                  </Button>
                </li>
              ))}
            </ul>
            <SheetFooter className="border-t">
              <div className="flex items-center justify-between">
                <span className="text-muted-foreground">Tạm tính</span>
                <span className="text-lg font-semibold">{formatVND(subtotal)}</span>
              </div>
              <Separator className="my-1" />
              <SheetClose asChild>
                <Button asChild size="lg" className="h-11">
                  <Link href="/checkout">Thanh toán</Link>
                </Button>
              </SheetClose>
              <SheetClose asChild>
                <Button asChild variant="outline" size="lg" className="h-11">
                  <Link href="/cart">Xem giỏ hàng</Link>
                </Button>
              </SheetClose>
            </SheetFooter>
          </>
        )}
      </SheetContent>
    </Sheet>
  );
}
