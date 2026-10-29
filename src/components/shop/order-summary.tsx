"use client";

import { Separator } from "@/components/ui/separator";
import { formatVND } from "@/lib/format";
import { shippingFeeFor } from "@/lib/mock-data";
import { cn } from "@/lib/utils";

export function OrderSummary({
  subtotal,
  children,
  className,
}: {
  subtotal: number;
  children?: React.ReactNode;
  className?: string;
}) {
  const shipping = shippingFeeFor(subtotal);
  return (
    <div className={cn("space-y-3 text-sm", className)}>
      <div className="flex justify-between">
        <span className="text-muted-foreground">Tạm tính</span>
        <span>{formatVND(subtotal)}</span>
      </div>
      <div className="flex justify-between">
        <span className="text-muted-foreground">Phí vận chuyển</span>
        <span>{shipping === 0 ? "Miễn phí" : formatVND(shipping)}</span>
      </div>
      {children}
      <Separator />
      <div className="flex items-baseline justify-between">
        <span className="font-medium">Tổng cộng</span>
        <span className="text-xl font-semibold text-primary">{formatVND(subtotal + shipping)}</span>
      </div>
      <p className="text-right text-xs text-muted-foreground">Đã bao gồm VAT</p>
    </div>
  );
}
