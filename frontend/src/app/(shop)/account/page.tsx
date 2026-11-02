import { Suspense } from "react";
import type { Metadata } from "next";
import { Heart, LogOut, MapPin, Package, User } from "lucide-react";
import { OrderStatusBadge } from "@/components/order-status-badge";
import { SignOutButton } from "@clerk/nextjs";
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Skeleton } from "@/components/ui/skeleton";
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table";
import { requireUser } from "@/lib/auth/dal";
import { formatDate, formatVND, initials } from "@/lib/format";
import { EditProfileButton } from "./edit-profile-button";
import { customers, getBookById, orderTotal, orders } from "@/lib/mock-data";

export const metadata: Metadata = { title: "Tài khoản của tôi" };

export default function AccountPage() {
  return (
    <Suspense fallback={<AccountSkeleton />}>
      <AccountContent />
    </Suspense>
  );
}

async function AccountContent() {
  const user = await requireUser("/account");
  // TODO: replace with the orders API once the backend exposes it. Until then the
  // order history is mock data for the demo customer.
  const myOrders = orders.filter((order) => order.customerId === customers[0].id);
  const totalSpent = myOrders.filter((o) => o.status !== "cancelled").reduce((sum, o) => sum + orderTotal(o), 0);

  return (
    <div className="container-page grid gap-8 py-10 lg:grid-cols-[260px_1fr]">
      <aside className="space-y-4">
        <div className="flex items-center gap-3">
          <Avatar className="size-14">
            <AvatarImage src={user.imageUrl} alt="" />
            <AvatarFallback className="bg-primary text-lg text-primary-foreground">
              {initials(user.givenName)}
            </AvatarFallback>
          </Avatar>
          <div className="min-w-0">
            <p className="truncate font-heading text-lg font-semibold">{user.name}</p>
            <p className="truncate text-sm text-muted-foreground">{user.email}</p>
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
          <SignOutButton redirectUrl="/">
            <Button variant="ghost" className="justify-start text-destructive hover:text-destructive">
              <LogOut /> Đăng xuất
            </Button>
          </SignOutButton>
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
            <EditProfileButton />
          </CardHeader>
          <CardContent>
            <dl className="grid gap-4 text-sm sm:grid-cols-2">
              {[
                ["Họ và tên", user.name],
                ["Email", user.email],
                ["Số điện thoại", user.phone ?? "Chưa cập nhật"],
                ["Địa chỉ mặc định", "Chưa cập nhật"],
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

function AccountSkeleton() {
  return (
    <div className="container-page grid gap-8 py-10 lg:grid-cols-[260px_1fr]" aria-busy>
      <div className="flex items-center gap-3">
        <Skeleton className="size-14 rounded-full" />
        <div className="space-y-2">
          <Skeleton className="h-5 w-32" />
          <Skeleton className="h-4 w-40" />
        </div>
      </div>
      <div className="space-y-8">
        <Skeleton className="h-10 w-64" />
        <div className="grid gap-4 sm:grid-cols-3">
          {[0, 1, 2].map((i) => (
            <Skeleton key={i} className="h-24" />
          ))}
        </div>
        <Skeleton className="h-48" />
      </div>
    </div>
  );
}
