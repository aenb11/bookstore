import type { Metadata } from "next";
import Link from "next/link";
import { Suspense } from "react";
import { SearchX } from "lucide-react";
import { BookGrid } from "@/components/book/book-card";
import { CatalogSidebar, CatalogToolbar } from "@/components/shop/catalog-filters";
import {
  Breadcrumb,
  BreadcrumbItem,
  BreadcrumbLink,
  BreadcrumbList,
  BreadcrumbPage,
  BreadcrumbSeparator,
} from "@/components/ui/breadcrumb";
import { Button } from "@/components/ui/button";
import {
  Pagination,
  PaginationContent,
  PaginationItem,
  PaginationLink,
  PaginationNext,
  PaginationPrevious,
} from "@/components/ui/pagination";
import { type CatalogParams, queryCatalog } from "@/lib/catalog";

export const metadata: Metadata = { title: "Tất cả sách" };

export default async function CatalogPage({ searchParams }: { searchParams: Promise<CatalogParams> }) {
  const params = await searchParams;
  const { category, total, page, totalPages, items } = queryCatalog(params);
  const title = params.q ? `Kết quả cho “${params.q}”` : (category?.name ?? "Tất cả sách");

  const pageHref = (target: number) => {
    const query = new URLSearchParams(params as Record<string, string>);
    query.set("page", String(target));
    return `/books?${query}`;
  };

  return (
    <div className="container-page py-8">
      <Breadcrumb>
        <BreadcrumbList>
          <BreadcrumbItem>
            <BreadcrumbLink asChild>
              <Link href="/">Trang chủ</Link>
            </BreadcrumbLink>
          </BreadcrumbItem>
          <BreadcrumbSeparator />
          {category ? (
            <>
              <BreadcrumbItem>
                <BreadcrumbLink asChild>
                  <Link href="/books">Sách</Link>
                </BreadcrumbLink>
              </BreadcrumbItem>
              <BreadcrumbSeparator />
              <BreadcrumbItem>
                <BreadcrumbPage>{category.name}</BreadcrumbPage>
              </BreadcrumbItem>
            </>
          ) : (
            <BreadcrumbItem>
              <BreadcrumbPage>Sách</BreadcrumbPage>
            </BreadcrumbItem>
          )}
        </BreadcrumbList>
      </Breadcrumb>

      <header className="mt-4 mb-8 max-w-2xl">
        <h1 className="text-3xl font-semibold sm:text-4xl">{title}</h1>
        {category && <p className="mt-2 text-muted-foreground">{category.description}</p>}
      </header>

      <div className="flex gap-10">
        <Suspense>
          <CatalogSidebar />
        </Suspense>
        <div className="min-w-0 flex-1 space-y-6">
          <Suspense>
            <CatalogToolbar total={total} />
          </Suspense>

          {items.length > 0 ? (
            <BookGrid books={items} className="lg:grid-cols-3 xl:grid-cols-4" />
          ) : (
            <div className="flex flex-col items-center gap-4 rounded-lg border border-dashed py-20 text-center">
              <SearchX className="size-10 text-muted-foreground" />
              <div>
                <p className="font-heading text-lg font-semibold">Không tìm thấy sách phù hợp</p>
                <p className="text-sm text-muted-foreground">Thử bỏ bớt bộ lọc hoặc tìm với từ khóa khác.</p>
              </div>
              <Button asChild variant="outline">
                <Link href="/books">Xem tất cả sách</Link>
              </Button>
            </div>
          )}

          {totalPages > 1 && (
            <Pagination className="pt-6">
              <PaginationContent>
                <PaginationItem>
                  <PaginationPrevious
                    text="Trước"
                    href={pageHref(page - 1)}
                    aria-disabled={page === 1}
                    className={page === 1 ? "pointer-events-none opacity-50" : undefined}
                  />
                </PaginationItem>
                {Array.from({ length: totalPages }, (_, i) => i + 1).map((n) => (
                  <PaginationItem key={n}>
                    <PaginationLink href={pageHref(n)} isActive={n === page}>
                      {n}
                    </PaginationLink>
                  </PaginationItem>
                ))}
                <PaginationItem>
                  <PaginationNext
                    text="Sau"
                    href={pageHref(page + 1)}
                    aria-disabled={page === totalPages}
                    className={page === totalPages ? "pointer-events-none opacity-50" : undefined}
                  />
                </PaginationItem>
              </PaginationContent>
            </Pagination>
          )}
        </div>
      </div>
    </div>
  );
}
