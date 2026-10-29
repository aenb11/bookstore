import type { Metadata } from "next";
import Link from "next/link";
import { CircleCheck } from "lucide-react";
import { Button } from "@/components/ui/button";
import { ClearCart } from "./clear-cart";

export const metadata: Metadata = { title: "Đặt hàng thành công" };

export default function CheckoutSuccessPage() {
  return (
    <div className="container-page flex flex-col items-center gap-5 py-24 text-center">
      <ClearCart />
      <span className="grid size-16 place-items-center rounded-full bg-secondary text-secondary-foreground">
        <CircleCheck className="size-8" />
      </span>
      <h1 className="text-3xl font-semibold sm:text-4xl">Cảm ơn bạn đã đặt hàng!</h1>
      <p className="max-w-md text-muted-foreground">
        Mã đơn hàng của bạn là <strong className="text-foreground">TS240151</strong>. Chúng tôi đã gửi email xác nhận
        và sẽ liên hệ trước khi giao hàng.
      </p>
      <div className="flex flex-wrap justify-center gap-3">
        <Button asChild size="lg" className="h-11 px-5">
          <Link href="/account#orders">Xem đơn hàng</Link>
        </Button>
        <Button asChild size="lg" variant="outline" className="h-11 px-5">
          <Link href="/books">Tiếp tục mua sắm</Link>
        </Button>
      </div>
    </div>
  );
}
