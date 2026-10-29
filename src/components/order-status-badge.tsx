import { Badge } from "@/components/ui/badge";
import { orderStatusLabel } from "@/lib/mock-data";
import type { OrderStatus } from "@/lib/types";
import { cn } from "@/lib/utils";

const statusClass: Record<OrderStatus, string> = {
  pending: "bg-amber-100 text-amber-900",
  shipping: "bg-sky-100 text-sky-900",
  completed: "bg-emerald-100 text-emerald-900",
  cancelled: "bg-stone-200 text-stone-700",
};

export function OrderStatusBadge({ status }: { status: OrderStatus }) {
  return (
    <Badge variant="outline" className={cn("border-transparent", statusClass[status])}>
      {orderStatusLabel[status]}
    </Badge>
  );
}
