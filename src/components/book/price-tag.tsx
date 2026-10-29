import { discountPercent, formatVND } from "@/lib/format";
import { cn } from "@/lib/utils";

type PriceTagProps = {
  price: number;
  originalPrice?: number;
  size?: "sm" | "lg";
  className?: string;
};

export function PriceTag({ price, originalPrice, size = "sm", className }: PriceTagProps) {
  const discount = discountPercent(price, originalPrice);
  return (
    <div className={cn("flex flex-wrap items-baseline gap-x-2 gap-y-0.5", className)}>
      <span className={cn("font-semibold text-primary", size === "lg" ? "text-3xl" : "text-base")}>
        {formatVND(price)}
      </span>
      {discount > 0 && (
        <>
          <span className={cn("text-muted-foreground line-through", size === "lg" ? "text-base" : "text-xs")}>
            <span className="sr-only">Giá gốc </span>
            {formatVND(originalPrice!)}
          </span>
          <span
            className={cn(
              "rounded-sm bg-primary/10 px-1 font-semibold text-primary",
              size === "lg" ? "text-sm" : "text-[0.7rem]",
            )}
          >
            -{discount}%
          </span>
        </>
      )}
    </div>
  );
}
