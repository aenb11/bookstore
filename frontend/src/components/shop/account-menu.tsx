import Link from "next/link";
import { LayoutDashboard, LogIn, LogOut, Package, User, UserPlus } from "lucide-react";
import { Button } from "@/components/ui/button";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import { SignOutButton } from "@clerk/nextjs";
import { getCurrentUser } from "@/lib/auth/dal";
import { ADMIN_AREA_ROLES, hasAnyRole } from "@/lib/auth/roles";

/** Header account dropdown. Reads the session, so render it inside <Suspense>. */
export async function AccountMenu() {
  const user = await getCurrentUser();

  return (
    <DropdownMenu>
      <DropdownMenuTrigger asChild>
        <AccountButton label={user ? user.givenName : "Tài khoản"} />
      </DropdownMenuTrigger>
      <DropdownMenuContent align="end" className="w-52">
        {user ? (
          <>
            <DropdownMenuLabel className="truncate">Xin chào, {user.givenName}</DropdownMenuLabel>
            <DropdownMenuSeparator />
            <DropdownMenuItem asChild>
              <Link href="/account">
                <User /> Tài khoản
              </Link>
            </DropdownMenuItem>
            <DropdownMenuItem asChild>
              <Link href="/account#orders">
                <Package /> Đơn hàng
              </Link>
            </DropdownMenuItem>
            {hasAnyRole(user, ADMIN_AREA_ROLES) && (
              <DropdownMenuItem asChild>
                <Link href="/admin">
                  <LayoutDashboard /> Trang quản trị
                </Link>
              </DropdownMenuItem>
            )}
            <DropdownMenuSeparator />
            <SignOutButton redirectUrl="/">
              <DropdownMenuItem variant="destructive">
                <LogOut /> Đăng xuất
              </DropdownMenuItem>
            </SignOutButton>
          </>
        ) : (
          <>
            <DropdownMenuItem asChild>
              <Link href="/login">
                <LogIn /> Đăng nhập
              </Link>
            </DropdownMenuItem>
            <DropdownMenuItem asChild>
              <Link href="/register">
                <UserPlus /> Đăng ký
              </Link>
            </DropdownMenuItem>
          </>
        )}
      </DropdownMenuContent>
    </DropdownMenu>
  );
}

/** Same trigger, shown while the session is being read. */
export function AccountMenuFallback() {
  return <AccountButton label="Tài khoản" disabled />;
}

function AccountButton({ label, ...props }: { label: string } & React.ComponentProps<typeof Button>) {
  return (
    <Button
      variant="ghost"
      className="h-12 flex-col gap-0.5 px-2 text-xs font-normal text-muted-foreground"
      {...props}
    >
      <User className="size-5" />
      <span className="hidden max-w-20 truncate sm:inline">{label}</span>
    </Button>
  );
}
