import type { Metadata } from "next";
import { Bell } from "lucide-react";
import { AdminSidebar } from "@/components/admin/admin-sidebar";
import { Avatar, AvatarFallback } from "@/components/ui/avatar";
import { Button } from "@/components/ui/button";
import { Separator } from "@/components/ui/separator";
import { SidebarInset, SidebarProvider, SidebarTrigger } from "@/components/ui/sidebar";

export const metadata: Metadata = {
  title: { default: "Quản trị", template: "%s | Quản trị Trang Sách" },
  robots: { index: false },
};

export default function AdminLayout({ children }: { children: React.ReactNode }) {
  return (
    <SidebarProvider>
      <AdminSidebar />
      <SidebarInset>
        <header className="sticky top-0 z-10 flex h-14 items-center gap-2 border-b bg-background/90 px-4 backdrop-blur">
          <SidebarTrigger aria-label="Thu gọn thanh bên" />
          <Separator orientation="vertical" className="mx-1 h-5" />
          <p className="text-sm text-muted-foreground">Chi nhánh Thủ Đức</p>
          <div className="ml-auto flex items-center gap-2">
            <Button variant="ghost" size="icon" aria-label="Thông báo" className="relative">
              <Bell />
              <span className="absolute top-1.5 right-1.5 size-2 rounded-full bg-primary" />
            </Button>
            <Avatar className="size-8">
              <AvatarFallback className="bg-secondary text-xs text-secondary-foreground">AD</AvatarFallback>
            </Avatar>
          </div>
        </header>
        <main className="flex-1 space-y-6 p-4 sm:p-6 lg:p-8">{children}</main>
      </SidebarInset>
    </SidebarProvider>
  );
}
