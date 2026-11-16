
"use client";

import { useMemo, useState } from "react";
import { Search, Users } from "lucide-react";

type Account = {
  id: number;
  name: string;
  email: string;
  role: "Admin" | "Customer";
  status: "Hoạt động" | "Đã khóa";
};

const initialAccounts: Account[] = [
  { id: 1, name: "Nguyễn Minh Anh", email: "minhanh@example.com", role: "Customer", status: "Hoạt động" },
  { id: 2, name: "Trần Hoàng Phúc", email: "hoangphuc@example.com", role: "Admin", status: "Hoạt động" },
  { id: 3, name: "Lê Thanh Hà", email: "thanhha@example.com", role: "Customer", status: "Đã khóa" },
  { id: 4, name: "Phạm Gia Hân", email: "giahan@example.com", role: "Customer", status: "Hoạt động" },
  { id: 5, name: "Võ Quốc Bảo", email: "quocbao@example.com", role: "Customer", status: "Hoạt động" },
];

export default function AccountsPage() {
  const [accounts, setAccounts] = useState(initialAccounts);
  const [search, setSearch] = useState("");
  const [roleFilter, setRoleFilter] = useState("all");

  const filteredAccounts = useMemo(() => {
    return accounts.filter((account) => {
      const keyword = search.toLowerCase().trim();

      const matchesSearch =
        account.name.toLowerCase().includes(keyword) ||
        account.email.toLowerCase().includes(keyword);

      const matchesRole =
        roleFilter === "all" || account.role === roleFilter;

      return matchesSearch && matchesRole;
    });
  }, [accounts, search, roleFilter]);

  function toggleStatus(id: number) {
    setAccounts((prev) =>
      prev.map((account) =>
        account.id === id
          ? {
              ...account,
              status:
                account.status === "Hoạt động"
                  ? "Đã khóa"
                  : "Hoạt động",
            }
          : account
      )
    );
  }

  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-2xl font-bold tracking-tight">
          Quản lý tài khoản
        </h1>
        <p className="mt-1 text-sm text-muted-foreground">
          Xem và quản lý tài khoản người dùng trong hệ thống Bookstore.
        </p>
      </div>

      <div className="grid gap-4 sm:grid-cols-3">
        <StatCard
          title="Tổng tài khoản"
          value={accounts.length}
        />
        <StatCard
          title="Đang hoạt động"
          value={accounts.filter((a) => a.status === "Hoạt động").length}
        />
        <StatCard
          title="Đã khóa"
          value={accounts.filter((a) => a.status === "Đã khóa").length}
        />
      </div>

      <div className="rounded-xl border bg-card shadow-sm">
        <div className="flex flex-col gap-3 border-b p-4 sm:flex-row">
          <div className="relative flex-1">
            <Search className="absolute left-3 top-1/2 size-4 -translate-y-1/2 text-muted-foreground" />
            <input
              type="text"
              placeholder="Tìm theo tên hoặc email..."
              value={search}
              onChange={(e) => setSearch(e.target.value)}
              className="h-10 w-full rounded-md border bg-background pl-9 pr-3 text-sm outline-none focus:ring-2 focus:ring-primary"
            />
          </div>

          <select
            value={roleFilter}
            onChange={(e) => setRoleFilter(e.target.value)}
            className="h-10 rounded-md border bg-background px-3 text-sm"
          >
            <option value="all">Tất cả vai trò</option>
            <option value="Admin">Admin</option>
            <option value="Customer">Customer</option>
          </select>
        </div>

        <div className="overflow-x-auto">
          <table className="w-full text-left text-sm">
            <thead className="border-b bg-muted/50">
              <tr>
                <th className="px-4 py-3 font-medium">ID</th>
                <th className="px-4 py-3 font-medium">Họ tên</th>
                <th className="px-4 py-3 font-medium">Email</th>
                <th className="px-4 py-3 font-medium">Vai trò</th>
                <th className="px-4 py-3 font-medium">Trạng thái</th>
                <th className="px-4 py-3 text-right font-medium">
                  Thao tác
                </th>
              </tr>
            </thead>

            <tbody>
              {filteredAccounts.map((account) => (
                <tr
                  key={account.id}
                  className="border-b last:border-0 hover:bg-muted/30"
                >
                  <td className="px-4 py-4">{account.id}</td>
                  <td className="px-4 py-4 font-medium">{account.name}</td>
                  <td className="px-4 py-4">{account.email}</td>
                  <td className="px-4 py-4">{account.role}</td>
                  <td className="px-4 py-4">
                    <span
                      className={`rounded-full px-2.5 py-1 text-xs font-medium ${
                        account.status === "Hoạt động"
                          ? "bg-green-100 text-green-700"
                          : "bg-red-100 text-red-700"
                      }`}
                    >
                      {account.status}
                    </span>
                  </td>
                  <td className="px-4 py-4 text-right">
                    <button
                      onClick={() => toggleStatus(account.id)}
                      className="rounded-md border px-3 py-1.5 text-xs font-medium hover:bg-muted"
                    >
                      {account.status === "Hoạt động"
                        ? "Khóa"
                        : "Mở khóa"}
                    </button>
                  </td>
                </tr>
              ))}

              {filteredAccounts.length === 0 && (
                <tr>
                  <td
                    colSpan={6}
                    className="px-4 py-10 text-center text-muted-foreground"
                  >
                    Không tìm thấy tài khoản phù hợp.
                  </td>
                </tr>
              )}
            </tbody>
          </table>
        </div>
      </div>

      <p className="flex items-center gap-2 text-xs text-muted-foreground">
        <Users className="size-4" />
        Dữ liệu minh họa. Các thao tác chưa kết nối Backend.
      </p>
    </div>
  );
}

function StatCard({
  title,
  value,
}: {
  title: string;
  value: number;
}) {
  return (
    <div className="rounded-xl border bg-card p-5 shadow-sm">
      <p className="text-sm text-muted-foreground">{title}</p>
      <p className="mt-2 text-3xl font-bold">{value}</p>
    </div>
  );
}
