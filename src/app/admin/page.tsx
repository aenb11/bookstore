import Link from "next/link";
import { AlertTriangle, ArrowUpRight, BookCopy, DollarSign, ShoppingCart, UserPlus } from "lucide-react";
import { PageHeader } from "@/components/admin/page-header";
import { RevenueChart } from "@/components/admin/revenue-chart";
import { BookCover } from "@/components/book/book-cover";
import { OrderStatusBadge } from "@/components/order-status-badge";
import { Button } from "@/components/ui/button";
import { Card, CardAction, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table";
import { formatDate, formatVND } from "@/lib/format";
import { LOW_STOCK_THRESHOLD, books, getCustomer, monthlyRevenue, orderTotal, orders } from "@/lib/mock-data";

export default function AdminDashboardPage() {
  const [prev, current] = monthlyRevenue.slice(-2);
  const lowStock = books.filter((book) => book.stock <= LOW_STOCK_THRESHOLD).sort((a, b) => a.stock - b.stock);
  const growth = (value: number, base: number) => `${value >= base ? "+" : ""}${(((value - base) / base) * 100).toFixed(1)}%`;

  const kpis = [
    { label: "Doanh thu tháng", value: formatVND(current.revenue), delta: growth(current.revenue, prev.revenue), icon: DollarSign },
    { label: "Đơn hàng", value: current.orders.toLocaleString("vi-VN"), delta: growth(current.orders, prev.orders), icon: ShoppingCart },
    { label: "Khách hàng mới", value: "312", delta: "+8.2%", icon: UserPlus },
    { label: "Đầu sách", value: books.length.toLocaleString("vi-VN"), delta: `${lowStock.length} sắp hết`, icon: BookCopy },
  ];

  return (
    <>
      <PageHeader title="Tổng quan" description="Tình hình kinh doanh tháng 10/2026" />

      <div className="grid gap-4 sm:grid-cols-2 xl:grid-cols-4">
        {kpis.map(({ label, value, delta, icon: Icon }) => (
          <Card key={label} className="gap-3">
            <CardHeader>
              <CardDescription>{label}</CardDescription>
              <CardAction>
                <span className="grid size-8 place-items-center rounded-md bg-accent text-accent-foreground">
                  <Icon className="size-4" />
                </span>
              </CardAction>
            </CardHeader>
            <CardContent>
              <p className="font-heading text-2xl font-semibold">{value}</p>
              <p className="mt-1 text-xs text-muted-foreground">
                <span className={delta.startsWith("+") ? "font-medium text-emerald-700" : "font-medium text-primary"}>
                  {delta}
                </span>{" "}
                {delta.startsWith("+") && "so với tháng trước"}
              </p>
            </CardContent>
          </Card>
        ))}
      </div>

      <div className="grid gap-4 xl:grid-cols-[2fr_1fr]">
        <Card>
          <CardHeader>
            <CardTitle className="font-heading text-lg">Doanh thu 6 tháng</CardTitle>
            <CardDescription>Đơn vị: VND</CardDescription>
          </CardHeader>
          <CardContent>
            <RevenueChart />
          </CardContent>
        </Card>

        <Card>
          <CardHeader>
            <CardTitle className="flex items-center gap-2 font-heading text-lg">
              <AlertTriangle className="size-4 text-primary" /> Sắp hết hàng
            </CardTitle>
            <CardDescription>Tồn kho ≤ {LOW_STOCK_THRESHOLD} cuốn</CardDescription>
          </CardHeader>
          <CardContent>
            <ul className="space-y-3">
              {lowStock.map((book) => (
                <li key={book.id} className="flex items-center gap-3">
                  <div className="w-9 shrink-0">
                    <BookCover book={book} size="xs" />
                  </div>
                  <div className="min-w-0 flex-1">
                    <p className="truncate text-sm font-medium">{book.title}</p>
                    <p className="truncate text-xs text-muted-foreground">{book.author}</p>
                  </div>
                  <span className="rounded-md bg-primary/10 px-2 py-0.5 text-xs font-semibold text-primary tabular-nums">
                    {book.stock}
                  </span>
                </li>
              ))}
            </ul>
          </CardContent>
        </Card>
      </div>

      <Card>
        <CardHeader>
          <CardTitle className="font-heading text-lg">Đơn hàng gần đây</CardTitle>
          <CardAction>
            <Button asChild variant="ghost" size="sm">
              <Link href="/admin/orders">
                Xem tất cả <ArrowUpRight />
              </Link>
            </Button>
          </CardAction>
        </CardHeader>
        <CardContent>
          <Table>
            <TableHeader>
              <TableRow>
                <TableHead>Mã đơn</TableHead>
                <TableHead>Khách hàng</TableHead>
                <TableHead className="hidden sm:table-cell">Ngày đặt</TableHead>
                <TableHead>Trạng thái</TableHead>
                <TableHead className="text-right">Tổng tiền</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {orders.slice(0, 6).map((order) => (
                <TableRow key={order.id}>
                  <TableCell className="font-medium">#{order.code}</TableCell>
                  <TableCell>{getCustomer(order.customerId)?.name}</TableCell>
                  <TableCell className="hidden sm:table-cell">{formatDate(order.createdAt)}</TableCell>
                  <TableCell>
                    <OrderStatusBadge status={order.status} />
                  </TableCell>
                  <TableCell className="text-right font-medium">{formatVND(orderTotal(order))}</TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>
        </CardContent>
      </Card>
    </>
  );
}
