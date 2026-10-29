"use client";

import { ShoppingBag } from "lucide-react";
import { toast } from "sonner";
import { Button } from "@/components/ui/button";
import { cart } from "@/lib/cart-store";
import type { Book } from "@/lib/types";

type AddToCartButtonProps = {
  book: Pick<Book, "id" | "title" | "stock">;
  quantity?: number;
  compact?: boolean;
} & Omit<React.ComponentProps<typeof Button>, "onClick">;

export function AddToCartButton({ book, quantity = 1, compact, ...props }: AddToCartButtonProps) {
  const soldOut = book.stock === 0;

  return (
    <Button
      {...props}
      disabled={soldOut || props.disabled}
      aria-label={compact ? `Thêm ${book.title} vào giỏ` : undefined}
      onClick={() => {
        cart.add(book.id, quantity);
        toast.success("Đã thêm vào giỏ hàng", { description: `${quantity} × ${book.title}` });
      }}
    >
      <ShoppingBag />
      {!compact && (soldOut ? "Hết hàng" : "Thêm vào giỏ")}
    </Button>
  );
}
