import type { Metadata } from "next";
import { PageHeader } from "@/components/admin/page-header";
import { Avatar, AvatarFallback } from "@/components/ui/avatar";
import { Card } from "@/components/ui/card";
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table";
import { formatDate, formatVND } from "@/lib/format";
import { customers, orderTotal, orders } from "@/lib/mock-data";

export const metadata: Metadata = { title: "Khách hàng" };

function initials(name: string) {
  const parts = name.split(" ");
  return (parts[0][0] + parts[parts.length - 1][0]).toUpperCase();
}

export default function AdminCustomersPage() {
  const rows = customers.map((customer) => {
    const own = orders.filter((order) => order.customerId === customer.id);
    const spent = own.filter((o) => o.status !== "cancelled").reduce((sum, o) => sum + orderTotal(o), 0);
    return { ...customer, orderCount: own.length, spent };
  });

  return (
    <>
      <PageHeader title="Khách hàng" description={`${customers.length} khách hàng đã đăng ký`} />
      <Card className="gap-0 py-0">
        <Table>
          <TableHeader>
            <TableRow>
              <TableHead className="pl-4">Khách hàng</TableHead>
              <TableHead className="hidden md:table-cell">Số điện thoại</TableHead>
              <TableHead className="hidden lg:table-cell">Thành phố</TableHead>
              <TableHead className="hidden sm:table-cell">Ngày tham gia</TableHead>
              <TableHead className="text-right">Đơn hàng</TableHead>
              <TableHead className="pr-4 text-right">Tổng chi tiêu</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {rows.map((customer) => (
              <TableRow key={customer.id}>
                <TableCell className="pl-4">
                  <div className="flex items-center gap-3">
                    <Avatar className="size-8">
                      <AvatarFallback className="bg-accent text-xs">{initials(customer.name)}</AvatarFallback>
                    </Avatar>
                    <div>
                      <p className="font-medium">{customer.name}</p>
                      <p className="text-xs text-muted-foreground">{customer.email}</p>
                    </div>
                  </div>
                </TableCell>
                <TableCell className="hidden md:table-cell">{customer.phone}</TableCell>
                <TableCell className="hidden lg:table-cell">{customer.city}</TableCell>
                <TableCell className="hidden sm:table-cell">{formatDate(customer.joinedAt)}</TableCell>
                <TableCell className="text-right tabular-nums">{customer.orderCount}</TableCell>
                <TableCell className="pr-4 text-right font-medium">{formatVND(customer.spent)}</TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </Card>
    </>
  );
}
