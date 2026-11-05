import { Suspense } from "react";
import type { Metadata } from "next";
import { Bell } from "lucide-react";
import { AdminSidebar } from "@/components/admin/admin-sidebar";
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar";
import { Button } from "@/components/ui/button";
import { Separator } from "@/components/ui/separator";
import { SidebarInset, SidebarProvider, SidebarTrigger } from "@/components/ui/sidebar";
import { Skeleton } from "@/components/ui/skeleton";
import { requireRole } from "@/lib/auth/dal";
import { ADMIN_AREA_ROLES } from "@/lib/auth/roles";
import { initials } from "@/lib/format";

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
            <Suspense fallback={<Skeleton className="size-8 rounded-full" />}>
              <AdminAvatar />
            </Suspense>
          </div>
        </header>
        <main className="flex-1 space-y-6 p-4 sm:p-6 lg:p-8">
          <Suspense fallback={<Skeleton className="h-64" />}>
            <AdminGuard>{children}</AdminGuard>
          </Suspense>
        </main>
      </SidebarInset>
    </SidebarProvider>
  );
}

/**
 * Server-side authorization for every admin page (the proxy redirect is only an
 * optimistic check). Admin pages render only after the role check passes.
 */
async function AdminGuard({ children }: { children: React.ReactNode }) {
  await requireRole(ADMIN_AREA_ROLES, "/admin");
  return children;
}

async function AdminAvatar() {
  const user = await requireRole(ADMIN_AREA_ROLES, "/admin");
  return (
    <Avatar className="size-8" title={`${user.name} (${user.email})`}>
      <AvatarImage src={user.imageUrl} alt="" />
      <AvatarFallback className="bg-secondary text-xs text-secondary-foreground">{initials(user.givenName)}</AvatarFallback>
    </Avatar>
  );
}
