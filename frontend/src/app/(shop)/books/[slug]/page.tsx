import type { Metadata } from "next";
import Link from "next/link";
import { notFound } from "next/navigation";
import { BadgeCheck, RotateCcw, Truck } from "lucide-react";
import { BookCard } from "@/components/book/book-card";
import { BookCover } from "@/components/book/book-cover";
import { PriceTag } from "@/components/book/price-tag";
import { PurchasePanel } from "@/components/book/purchase-panel";
import { RatingStars } from "@/components/book/rating-stars";
import { Avatar, AvatarFallback } from "@/components/ui/avatar";
import {
  Breadcrumb,
  BreadcrumbItem,
  BreadcrumbLink,
  BreadcrumbList,
  BreadcrumbPage,
  BreadcrumbSeparator,
} from "@/components/ui/breadcrumb";
import { Separator } from "@/components/ui/separator";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { formatDate } from "@/lib/format";
import { LOW_STOCK_THRESHOLD, books, getBookBySlug, getCategory, reviews } from "@/lib/mock-data";

type Props = { params: Promise<{ slug: string }> };

export function generateStaticParams() {
  return books.map((book) => ({ slug: book.slug }));
}

export async function generateMetadata({ params }: Props): Promise<Metadata> {
  const book = getBookBySlug((await params).slug);
  return book ? { title: book.title, description: book.description.slice(0, 155) } : {};
}

export default async function BookDetailPage({ params }: Props) {
  const book = getBookBySlug((await params).slug);
  if (!book) notFound();

  const category = getCategory(book.categoryId)!;
  const related = books.filter((b) => b.categoryId === book.categoryId && b.id !== book.id).slice(0, 5);
  const details = [
    ["Tác giả", book.author],
    ...(book.translator ? [["Dịch giả", book.translator]] : []),
    ["Nhà xuất bản", book.publisher],
    ["Năm xuất bản", String(book.publishedYear)],
    ["Số trang", String(book.pages)],
    ["ISBN", book.isbn],
    ["Danh mục", category.name],
  ];

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
          <BreadcrumbItem>
            <BreadcrumbLink asChild>
              <Link href={`/books?category=${category.slug}`}>{category.name}</Link>
            </BreadcrumbLink>
          </BreadcrumbItem>
          <BreadcrumbSeparator />
          <BreadcrumbItem>
            <BreadcrumbPage className="line-clamp-1">{book.title}</BreadcrumbPage>
          </BreadcrumbItem>
        </BreadcrumbList>
      </Breadcrumb>

      <div className="mt-4 grid gap-10 rounded-lg bg-card p-4 sm:p-6 md:grid-cols-[minmax(0,2fr)_minmax(0,3fr)] lg:gap-16">
        <div className="rounded-xl bg-muted/60 px-[18%] py-[12%] md:sticky md:top-32 md:self-start">
          <BookCover book={book} size="lg" />
        </div>

        <div className="space-y-6">
          <div className="space-y-3">
            <Link
              href={`/books?category=${category.slug}`}
              className="text-sm font-semibold tracking-wider text-primary uppercase hover:underline"
            >
              {category.name}
            </Link>
            <h1 className="text-3xl leading-tight font-semibold sm:text-4xl">{book.title}</h1>
            <p className="text-muted-foreground">
              <span className="text-foreground">{book.author}</span>
              {book.translator && <> · Dịch: {book.translator}</>}
            </p>
            <div className="flex flex-wrap items-center gap-x-4 gap-y-1 text-sm">
              <RatingStars rating={book.rating} size="md" />
              <span>
                <strong>{book.rating.toFixed(1)}</strong>{" "}
                <span className="text-muted-foreground">({book.reviewCount.toLocaleString("vi-VN")} đánh giá)</span>
              </span>
              <span className="text-muted-foreground">Đã bán {book.soldCount.toLocaleString("vi-VN")}</span>
            </div>
          </div>

          <div className="space-y-3 rounded-xl border bg-card p-5">
            <PriceTag price={book.price} originalPrice={book.originalPrice} size="lg" />
            <p className="text-sm">
              {book.stock === 0 ? (
                <span className="font-medium text-destructive">Tạm hết hàng</span>
              ) : book.stock <= LOW_STOCK_THRESHOLD ? (
                <span className="font-medium text-primary">Chỉ còn {book.stock} cuốn — đặt ngay kẻo lỡ</span>
              ) : (
                <span className="text-emerald-700">Còn hàng</span>
              )}
            </p>
            <PurchasePanel book={book} />
          </div>

          <ul className="grid gap-3 text-sm sm:grid-cols-3">
            <li className="flex items-center gap-2">
              <Truck className="size-4 text-primary" /> Giao 2–4 ngày
            </li>
            <li className="flex items-center gap-2">
              <RotateCcw className="size-4 text-primary" /> Đổi trả 7 ngày
            </li>
            <li className="flex items-center gap-2">
              <BadgeCheck className="size-4 text-primary" /> Sách chính hãng
            </li>
          </ul>

          <Separator />

          <Tabs defaultValue="description">
            <TabsList>
              <TabsTrigger value="description">Mô tả</TabsTrigger>
              <TabsTrigger value="details">Thông tin chi tiết</TabsTrigger>
              <TabsTrigger value="reviews">Đánh giá ({reviews.length})</TabsTrigger>
            </TabsList>
            <TabsContent value="description" className="pt-4">
              <p className="max-w-prose leading-relaxed text-pretty">{book.description}</p>
            </TabsContent>
            <TabsContent value="details" className="pt-4">
              <dl className="grid max-w-lg grid-cols-[auto_1fr] gap-x-8 gap-y-2 text-sm">
                {details.map(([term, value]) => (
                  <div key={term} className="contents">
                    <dt className="text-muted-foreground">{term}</dt>
                    <dd>{value}</dd>
                  </div>
                ))}
              </dl>
            </TabsContent>
            <TabsContent value="reviews" className="space-y-5 pt-4">
              {reviews.map((review) => (
                <article key={review.id} className="flex gap-3">
                  <Avatar>
                    <AvatarFallback>{review.author.charAt(0)}</AvatarFallback>
                  </Avatar>
                  <div className="space-y-1">
                    <div className="flex flex-wrap items-center gap-x-3">
                      <p className="font-medium">{review.author}</p>
                      <RatingStars rating={review.rating} />
                      <time dateTime={review.createdAt} className="text-xs text-muted-foreground">
                        {formatDate(review.createdAt)}
                      </time>
                    </div>
                    <p className="text-sm leading-relaxed">{review.content}</p>
                  </div>
                </article>
              ))}
            </TabsContent>
          </Tabs>
        </div>
      </div>

      {related.length > 0 && (
        <section aria-labelledby="related-heading" className="mt-4 rounded-lg bg-card p-4 sm:p-6">
          <h2 id="related-heading" className="mb-6 text-2xl font-semibold sm:text-3xl">
            Có thể bạn cũng thích
          </h2>
          <div className="grid grid-cols-2 gap-2 sm:grid-cols-3 lg:grid-cols-5">
            {related.map((b) => (
              <BookCard key={b.id} book={b} />
            ))}
          </div>
        </section>
      )}
    </div>
  );
}
