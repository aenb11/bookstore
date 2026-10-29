import type { Metadata } from "next";
import { PageHeader } from "@/components/admin/page-header";
import { OrdersTable } from "./orders-table";

export const metadata: Metadata = { title: "Đơn hàng" };

export default function AdminOrdersPage() {
  return (
    <>
      <PageHeader title="Đơn hàng" description="Theo dõi và xử lý đơn hàng của khách." />
      <OrdersTable />
    </>
  );
}
