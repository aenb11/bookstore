import type { Metadata } from "next";
import { Heart, LogOut, MapPin, Package, User } from "lucide-react";
import { OrderStatusBadge } from "@/components/order-status-badge";
import { Avatar, AvatarFallback } from "@/components/ui/avatar";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table";
import { formatDate, formatVND } from "@/lib/format";
import { customers, getBookById, orderTotal, orders } from "@/lib/mock-data";

export const metadata: Metadata = { title: "Tài khoản của tôi" };

export default function AccountPage() {
  const me = customers[0];
  const myOrders = orders.filter((order) => order.customerId === me.id);
  const totalSpent = myOrders.filter((o) => o.status !== "cancelled").reduce((sum, o) => sum + orderTotal(o), 0);

  return (
    <div className="container-page grid gap-8 py-10 lg:grid-cols-[260px_1fr]">
      <aside className="space-y-4">
        <div className="flex items-center gap-3">
          <Avatar className="size-14">
            <AvatarFallback className="bg-primary text-lg text-primary-foreground">MA</AvatarFallback>
          </Avatar>
          <div>
            <p className="font-heading text-lg font-semibold">{me.name}</p>
            <p className="text-sm text-muted-foreground">Thành viên từ {formatDate(me.joinedAt)}</p>
          </div>
        </div>
        <nav aria-label="Tài khoản" className="flex flex-col gap-1">
          {[
            { icon: User, label: "Thông tin cá nhân", href: "#profile" },
            { icon: Package, label: "Đơn hàng của tôi", href: "#orders" },
            { icon: MapPin, label: "Sổ địa chỉ", href: "#profile" },
            { icon: Heart, label: "Sách yêu thích", href: "#" },
          ].map(({ icon: Icon, label, href }) => (
            <Button key={label} asChild variant="ghost" className="justify-start">
              <a href={href}>
                <Icon /> {label}
              </a>
            </Button>
          ))}
          <Button variant="ghost" className="justify-start text-destructive hover:text-destructive">
            <LogOut /> Đăng xuất
          </Button>
        </nav>
      </aside>

      <div className="space-y-8">
        <h1 className="text-3xl font-semibold sm:text-4xl">Tài khoản của tôi</h1>

        <div className="grid gap-4 sm:grid-cols-3">
          {[
            { label: "Tổng đơn hàng", value: myOrders.length },
            { label: "Đã chi tiêu", value: formatVND(totalSpent) },
            { label: "Điểm thưởng", value: Math.floor(totalSpent / 10000).toLocaleString("vi-VN") },
          ].map((stat) => (
            <Card key={stat.label} className="gap-1 py-5">
              <CardContent>
                <p className="text-sm text-muted-foreground">{stat.label}</p>
                <p className="mt-1 font-heading text-2xl font-semibold">{stat.value}</p>
              </CardContent>
            </Card>
          ))}
        </div>

        <Card id="profile" className="scroll-mt-32">
          <CardHeader className="flex-row items-center justify-between">
            <CardTitle className="font-heading text-xl">Thông tin cá nhân</CardTitle>
            <Button variant="outline" size="sm">
              Chỉnh sửa
            </Button>
          </CardHeader>
          <CardContent>
            <dl className="grid gap-4 text-sm sm:grid-cols-2">
              {[
                ["Họ và tên", me.name],
                ["Email", me.email],
                ["Số điện thoại", me.phone],
                ["Địa chỉ mặc định", `12 Nguyễn Văn Cừ, ${me.city}`],
              ].map(([term, value]) => (
                <div key={term}>
                  <dt className="text-muted-foreground">{term}</dt>
                  <dd className="mt-0.5 font-medium">{value}</dd>
                </div>
              ))}
            </dl>
          </CardContent>
        </Card>

        <Card id="orders" className="scroll-mt-32">
          <CardHeader>
            <CardTitle className="font-heading text-xl">Đơn hàng gần đây</CardTitle>
          </CardHeader>
          <CardContent>
            <Table>
              <TableHeader>
                <TableRow>
                  <TableHead>Mã đơn</TableHead>
                  <TableHead>Ngày đặt</TableHead>
                  <TableHead className="hidden md:table-cell">Sản phẩm</TableHead>
                  <TableHead>Trạng thái</TableHead>
                  <TableHead className="text-right">Tổng tiền</TableHead>
                </TableRow>
              </TableHeader>
              <TableBody>
                {myOrders.map((order) => (
                  <TableRow key={order.id}>
                    <TableCell className="font-medium">#{order.code}</TableCell>
                    <TableCell>{formatDate(order.createdAt)}</TableCell>
                    <TableCell className="hidden max-w-60 truncate md:table-cell">
                      {order.items.map((item) => getBookById(item.bookId)?.title).join(", ")}
                    </TableCell>
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
      </div>
    </div>
  );
}
