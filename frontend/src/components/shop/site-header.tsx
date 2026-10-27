import Link from "next/link";
import { ChevronDown, LayoutGrid, Menu, Package, User } from "lucide-react";
import { Button } from "@/components/ui/button";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import { Sheet, SheetClose, SheetContent, SheetHeader, SheetTitle, SheetTrigger } from "@/components/ui/sheet";
import { categories } from "@/lib/mock-data";
import { CartSheet } from "./cart-sheet";
import { Logo } from "./logo";
import { SearchForm } from "./search-form";

const navLinks = [
  { href: "/books?sort=newest", label: "Sách mới" },
  { href: "/books?sort=bestselling", label: "Bán chạy" },
  { href: "/books?sale=1", label: "Khuyến mãi" },
];

export function SiteHeader() {
  return (
    <header className="sticky top-0 z-40 bg-card shadow-[0_1px_0_var(--border)]">
      <div className="bg-primary text-primary-foreground">
        <p className="container-page py-1.5 text-center text-xs font-medium">
          Miễn phí vận chuyển cho đơn từ 300.000 ₫ · Đổi trả trong 7 ngày
        </p>
      </div>
      <div className="container-page flex h-16 items-center gap-3 lg:gap-5">
        <MobileNav />
        <Logo />
        <DropdownMenu>
          <DropdownMenuTrigger asChild>
            <Button variant="ghost" size="lg" className="hidden text-muted-foreground lg:inline-flex">
              <LayoutGrid className="size-5" /> Danh mục <ChevronDown />
            </Button>
          </DropdownMenuTrigger>
          <DropdownMenuContent align="start" className="w-56">
            {categories.map((category) => (
              <DropdownMenuItem key={category.id} asChild>
                <Link href={`/books?category=${category.slug}`}>{category.name}</Link>
              </DropdownMenuItem>
            ))}
            <DropdownMenuSeparator />
            <DropdownMenuItem asChild>
              <Link href="/books">Tất cả sách</Link>
            </DropdownMenuItem>
          </DropdownMenuContent>
        </DropdownMenu>
        <SearchForm className="hidden flex-1 md:flex" />
        <div className="ml-auto flex items-center gap-1 md:ml-0">
          <DropdownMenu>
            <DropdownMenuTrigger asChild>
              <Button variant="ghost" className="h-12 flex-col gap-0.5 px-2 text-xs font-normal text-muted-foreground">
                <User className="size-5" />
                <span className="hidden sm:inline">Tài khoản</span>
              </Button>
            </DropdownMenuTrigger>
            <DropdownMenuContent align="end" className="w-48">
              <DropdownMenuLabel>Xin chào, Minh Anh</DropdownMenuLabel>
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
              <DropdownMenuSeparator />
              <DropdownMenuItem asChild>
                <Link href="/login">Đăng nhập</Link>
              </DropdownMenuItem>
              <DropdownMenuItem asChild>
                <Link href="/admin">Trang quản trị</Link>
              </DropdownMenuItem>
            </DropdownMenuContent>
          </DropdownMenu>
          <CartSheet />
        </div>
      </div>
      <div className="container-page pb-3 md:hidden">
        <SearchForm />
      </div>
    </header>
  );
}

function MobileNav() {
  return (
    <Sheet>
      <SheetTrigger asChild>
        <Button variant="ghost" size="icon-lg" className="lg:hidden" aria-label="Mở menu">
          <Menu className="size-5" />
        </Button>
      </SheetTrigger>
      <SheetContent side="left" className="w-80">
        <SheetHeader>
          <SheetTitle className="font-heading text-xl">Trang Sách</SheetTitle>
        </SheetHeader>
        <nav aria-label="Menu di động" className="flex flex-col gap-6 overflow-y-auto px-4 pb-6">
          <ul className="space-y-1">
            {navLinks.map((link) => (
              <li key={link.href}>
                <SheetClose asChild>
                  <Link href={link.href} className="block rounded-md px-2 py-2 font-medium hover:bg-muted">
                    {link.label}
                  </Link>
                </SheetClose>
              </li>
            ))}
          </ul>
          <div>
            <p className="mb-2 px-2 text-xs font-semibold tracking-wider text-muted-foreground uppercase">Danh mục</p>
            <ul className="space-y-1">
              {categories.map((category) => (
                <li key={category.id}>
                  <SheetClose asChild>
                    <Link
                      href={`/books?category=${category.slug}`}
                      className="block rounded-md px-2 py-2 hover:bg-muted"
                    >
                      {category.name}
                    </Link>
                  </SheetClose>
                </li>
              ))}
            </ul>
          </div>
        </nav>
      </SheetContent>
    </Sheet>
  );
}
