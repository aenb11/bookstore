"use client";

import { useState } from "react";
import { BookCover } from "@/components/book/book-cover";
import { OrderStatusBadge } from "@/components/order-status-badge";
import { Button } from "@/components/ui/button";
import { Card } from "@/components/ui/card";
import { Separator } from "@/components/ui/separator";
import {
  Sheet,
  SheetContent,
  SheetDescription,
  SheetFooter,
  SheetHeader,
  SheetTitle,
} from "@/components/ui/sheet";
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table";
import { Tabs, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { formatDate, formatVND } from "@/lib/format";
import {
  getBookById,
  getCustomer,
  orderStatusLabel,
  orderTotal,
  orders,
  paymentMethodLabel,
} from "@/lib/mock-data";
import type { Order, OrderStatus } from "@/lib/types";

const tabs: ("all" | OrderStatus)[] = ["all", "pending", "shipping", "completed", "cancelled"];

export function OrdersTable() {
  const [status, setStatus] = useState<"all" | OrderStatus>("all");
  const [selected, setSelected] = useState<Order | null>(null);
  const rows = orders.filter((order) => status === "all" || order.status === status);

  return (
    <>
      <Tabs value={status} onValueChange={(value) => setStatus(value as typeof status)}>
        <TabsList className="max-w-full overflow-x-auto">
          {tabs.map((tab) => (
            <TabsTrigger key={tab} value={tab}>
              {tab === "all" ? "Tất cả" : orderStatusLabel[tab]}
              <span className="ml-1 text-xs text-muted-foreground tabular-nums">
                {tab === "all" ? orders.length : orders.filter((o) => o.status === tab).length}
              </span>
            </TabsTrigger>
          ))}
        </TabsList>
      </Tabs>

      <Card className="gap-0 py-0">
        <Table>
          <TableHeader>
            <TableRow>
              <TableHead className="pl-4">Mã đơn</TableHead>
              <TableHead>Khách hàng</TableHead>
              <TableHead className="hidden md:table-cell">Ngày đặt</TableHead>
              <TableHead className="hidden lg:table-cell">Thanh toán</TableHead>
              <TableHead>Trạng thái</TableHead>
              <TableHead className="text-right">Tổng tiền</TableHead>
              <TableHead className="pr-4">
                <span className="sr-only">Chi tiết</span>
              </TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {rows.map((order) => (
              <TableRow key={order.id}>
                <TableCell className="pl-4 font-medium">#{order.code}</TableCell>
                <TableCell>{getCustomer(order.customerId)?.name}</TableCell>
                <TableCell className="hidden md:table-cell">{formatDate(order.createdAt)}</TableCell>
                <TableCell className="hidden lg:table-cell">{paymentMethodLabel[order.paymentMethod]}</TableCell>
                <TableCell>
                  <OrderStatusBadge status={order.status} />
                </TableCell>
                <TableCell className="text-right font-medium">{formatVND(orderTotal(order))}</TableCell>
                <TableCell className="pr-4 text-right">
                  <Button variant="outline" size="sm" onClick={() => setSelected(order)}>
                    Chi tiết
                  </Button>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </Card>

      <Sheet open={selected !== null} onOpenChange={(open) => !open && setSelected(null)}>
        <SheetContent className="w-full sm:max-w-md">
          {selected && <OrderDetail order={selected} />}
        </SheetContent>
      </Sheet>
    </>
  );
}

function OrderDetail({ order }: { order: Order }) {
  const customer = getCustomer(order.customerId);
  return (
    <>
      <SheetHeader>
        <SheetTitle className="font-heading text-xl">Đơn #{order.code}</SheetTitle>
        <SheetDescription>Đặt ngày {formatDate(order.createdAt)}</SheetDescription>
        <div>
          <OrderStatusBadge status={order.status} />
        </div>
      </SheetHeader>
      <div className="flex-1 space-y-6 overflow-y-auto px-4 text-sm">
        <section className="space-y-1">
          <h3 className="font-sans font-semibold">Khách hàng</h3>
          <p>{customer?.name}</p>
          <p className="text-muted-foreground">
            {customer?.phone} · {customer?.email}
          </p>
          <p className="text-muted-foreground">{order.shippingAddress}</p>
        </section>
        <Separator />
        <section className="space-y-3">
          <h3 className="font-sans font-semibold">Sản phẩm</h3>
          <ul className="space-y-3">
            {order.items.map((item) => {
              const book = getBookById(item.bookId)!;
              return (
                <li key={item.bookId} className="flex items-center gap-3">
                  <div className="w-10 shrink-0">
                    <BookCover book={book} size="xs" />
                  </div>
                  <div className="min-w-0 flex-1">
                    <p className="truncate font-medium">{book.title}</p>
                    <p className="text-muted-foreground">
                      {item.quantity} × {formatVND(item.unitPrice)}
                    </p>
                  </div>
                  <span className="font-medium">{formatVND(item.quantity * item.unitPrice)}</span>
                </li>
              );
            })}
          </ul>
        </section>
        <Separator />
        <dl className="space-y-2">
          <div className="flex justify-between">
            <dt className="text-muted-foreground">Phí vận chuyển</dt>
            <dd>{order.shippingFee === 0 ? "Miễn phí" : formatVND(order.shippingFee)}</dd>
          </div>
          <div className="flex justify-between">
            <dt className="text-muted-foreground">Thanh toán</dt>
            <dd>{paymentMethodLabel[order.paymentMethod]}</dd>
          </div>
          <div className="flex justify-between text-base font-semibold">
            <dt>Tổng cộng</dt>
            <dd className="text-primary">{formatVND(orderTotal(order))}</dd>
          </div>
        </dl>
      </div>
      {order.status === "pending" && (
        <SheetFooter className="flex-row border-t">
          <Button variant="outline" className="flex-1">
            Hủy đơn
          </Button>
          <Button className="flex-1">Xác nhận &amp; giao hàng</Button>
        </SheetFooter>
      )}
    </>
  );
}
