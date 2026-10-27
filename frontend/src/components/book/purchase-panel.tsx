"use client";

import { useState } from "react";
import { useRouter } from "next/navigation";
import { Minus, Plus, Zap } from "lucide-react";
import { Button } from "@/components/ui/button";
import { cart } from "@/lib/cart-store";
import type { Book } from "@/lib/types";
import { AddToCartButton } from "./add-to-cart-button";

export function PurchasePanel({ book }: { book: Pick<Book, "id" | "title" | "stock"> }) {
  const [quantity, setQuantity] = useState(1);
  const router = useRouter();
  const soldOut = book.stock === 0;

  return (
    <div className="flex flex-wrap items-center gap-3">
      <div className="inline-flex h-11 items-center rounded-md border bg-card" role="group" aria-label="Số lượng">
        <Button
          variant="ghost"
          size="icon-lg"
          aria-label="Giảm số lượng"
          disabled={quantity <= 1}
          onClick={() => setQuantity((q) => q - 1)}
        >
          <Minus />
        </Button>
        <span className="w-10 text-center tabular-nums" aria-live="polite">
          {quantity}
        </span>
        <Button
          variant="ghost"
          size="icon-lg"
          aria-label="Tăng số lượng"
          disabled={quantity >= book.stock}
          onClick={() => setQuantity((q) => q + 1)}
        >
          <Plus />
        </Button>
      </div>
      <AddToCartButton book={book} quantity={quantity} size="lg" variant="outline" className="h-11 flex-1 px-5 sm:flex-none" />
      <Button
        size="lg"
        className="h-11 flex-1 px-6 sm:flex-none"
        disabled={soldOut}
        onClick={() => {
          cart.add(book.id, quantity);
          router.push("/checkout");
        }}
      >
        <Zap /> Mua ngay
      </Button>
    </div>
  );
}
