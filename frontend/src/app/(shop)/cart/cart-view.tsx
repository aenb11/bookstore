"use client";

import Link from "next/link";
import { ShoppingBag, Tag, Trash2 } from "lucide-react";
import { BookCover } from "@/components/book/book-cover";
import { QuantityStepper, useCartLines } from "@/components/shop/cart-sheet";
import { OrderSummary } from "@/components/shop/order-summary";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { cart } from "@/lib/cart-store";
import { formatVND } from "@/lib/format";

export function CartView() {
  const { lines, subtotal } = useCartLines();

  if (lines.length === 0) {
    return (
      <div className="flex flex-col items-center gap-4 rounded-xl border border-dashed py-20 text-center">
        <ShoppingBag className="size-12 text-muted-foreground/50" />
        <div>
          <p className="font-heading text-xl font-semibold">Giỏ hàng đang trống</p>
          <p className="text-muted-foreground">Hãy chọn cho mình vài cuốn sách hay nhé.</p>
        </div>
        <Button asChild size="lg" className="h-11 px-5">
          <Link href="/books">Tiếp tục mua sắm</Link>
        </Button>
      </div>
    );
  }

  return (
    <div className="grid gap-8 lg:grid-cols-[1fr_380px]">
      <div className="rounded-xl border bg-card">
        <div className="hidden grid-cols-[1fr_140px_120px_40px] gap-4 border-b px-5 py-3 text-xs font-semibold tracking-wider text-muted-foreground uppercase sm:grid">
          <span>Sản phẩm</span>
          <span>Số lượng</span>
          <span className="text-right">Thành tiền</span>
          <span className="sr-only">Xóa</span>
        </div>
        <ul className="divide-y">
          {lines.map(({ book, quantity }) => (
            <li
              key={book.id}
              className="grid grid-cols-[1fr_auto] items-center gap-4 px-5 py-4 sm:grid-cols-[1fr_140px_120px_40px]"
            >
              <div className="flex items-center gap-4">
                <div className="w-16 shrink-0">
                  <BookCover book={book} size="sm" />
                </div>
                <div className="min-w-0">
                  <Link href={`/books/${book.slug}`} className="line-clamp-2 font-heading font-semibold hover:underline">
                    {book.title}
                  </Link>
                  <p className="text-sm text-muted-foreground">{book.author}</p>
                  <p className="text-sm">{formatVND(book.price)}</p>
                </div>
              </div>
              <div className="col-start-1 sm:col-start-auto">
                <QuantityStepper bookId={book.id} quantity={quantity} max={book.stock} />
              </div>
              <p className="row-start-1 text-right font-semibold sm:row-start-auto">{formatVND(book.price * quantity)}</p>
              <Button
                variant="ghost"
                size="icon"
                className="justify-self-end"
                aria-label={`Xóa ${book.title}`}
                onClick={() => cart.remove(book.id)}
              >
                <Trash2 />
              </Button>
            </li>
          ))}
        </ul>
      </div>

      <Card className="h-fit lg:sticky lg:top-32">
        <CardHeader>
          <CardTitle className="font-heading text-xl">Tóm tắt đơn hàng</CardTitle>
        </CardHeader>
        <CardContent className="space-y-5">
          <form className="flex gap-2" aria-label="Mã giảm giá">
            <div className="relative flex-1">
              <Tag className="pointer-events-none absolute top-1/2 left-3 size-4 -translate-y-1/2 text-muted-foreground" />
              <Input placeholder="Mã giảm giá" aria-label="Mã giảm giá" className="h-10 pl-9" />
            </div>
            <Button type="button" variant="outline" className="h-10">
              Áp dụng
            </Button>
          </form>
          <OrderSummary subtotal={subtotal} />
          <Button asChild size="lg" className="h-11 w-full">
            <Link href="/checkout">Tiến hành thanh toán</Link>
          </Button>
          <Button asChild variant="link" className="w-full">
            <Link href="/books">Tiếp tục mua sắm</Link>
          </Button>
        </CardContent>
      </Card>
    </div>
  );
}
