import Link from "next/link";
import {
  Atom,
  BookHeart,
  Brain,
  ChevronRight,
  Globe,
  Landmark,
  Rabbit,
  Sprout,
  TrendingUp,
  type LucideIcon,
} from "lucide-react";
import { BookCard } from "@/components/book/book-card";
import { BookCover } from "@/components/book/book-cover";
import { type Banner, BannerCarousel } from "@/components/shop/home/banner-carousel";
import { FlashSale } from "@/components/shop/home/flash-sale";
import { Ranking } from "@/components/shop/home/ranking";
import { books, categories } from "@/lib/mock-data";

const categoryIcons: Record<string, LucideIcon> = {
  c1: BookHeart,
  c2: Globe,
  c3: TrendingUp,
  c4: Sprout,
  c5: Rabbit,
  c6: Landmark,
  c7: Atom,
  c8: Brain,
};

const bySlug = (slug: string) => books.find((book) => book.slug === slug)!;

export default function HomePage() {
  const bestsellers = [...books].sort((a, b) => b.soldCount - a.soldCount);
  const onSale = books.filter((book) => book.originalPrice).slice(0, 6);

  const banners: Banner[] = [
    {
      title: "Tháng 10 rộn ràng — giảm đến 25%",
      subtitle: "Hàng nghìn đầu sách văn học, kỹ năng đang giảm giá.",
      href: "/books?sale=1",
      className: "bg-primary text-primary-foreground",
      books: [bySlug("nha-gia-kim"), bySlug("dac-nhan-tam"), bySlug("tu-tot-den-vi-dai")],
    },
    {
      title: "Nguyễn Nhật Ánh — tuyển tập",
      subtitle: "Những trang sách tuổi thơ, nay có ấn bản mới.",
      href: "/books?q=Nguy%E1%BB%85n%20Nh%E1%BA%ADt%20%C3%81nh",
      className: "bg-[oklch(0.32_0.06_255)] text-white",
      books: [bySlug("mat-biec"), bySlug("toi-thay-hoa-vang-tren-co-xanh"), bySlug("hoang-tu-be")],
    },
    {
      title: "Sách thiếu nhi đồng giá từ 25K",
      subtitle: "Truyện tranh, cổ tích và sách tô màu cho bé.",
      href: "/books?category=thieu-nhi",
      className: "bg-[oklch(0.86_0.12_85)] text-foreground",
      books: [bySlug("doraemon-tap-1"), bySlug("de-men-phieu-luu-ky"), bySlug("hoang-tu-be")],
    },
  ];

  const rankingGroups = ["c1", "c2", "c3", "c4"].map((id) => ({
    category: categories.find((c) => c.id === id)!,
    books: bestsellers.filter((book) => book.categoryId === id).slice(0, 5),
  }));
  // Pad short categories with overall bestsellers so every tab shows 5 rows.
  for (const group of rankingGroups) {
    for (const book of bestsellers) {
      if (group.books.length >= 5) break;
      if (!group.books.includes(book)) group.books.push(book);
    }
  }

  return (
    <div className="container-page space-y-4 py-4">
      {/* Category menu + banners */}
      <section className="grid gap-4 lg:h-80 lg:grid-cols-[220px_minmax(0,1fr)_260px]">
        <nav aria-label="Danh mục sản phẩm" className="hidden rounded-lg bg-card py-2 lg:block">
          <p className="px-4 pt-1 pb-2 text-sm font-bold">Danh mục sản phẩm</p>
          <ul>
            {categories.map((category) => {
              const Icon = categoryIcons[category.id];
              return (
                <li key={category.id}>
                  <Link
                    href={`/books?category=${category.slug}`}
                    className="flex items-center gap-3 px-4 py-1.5 text-sm hover:bg-muted hover:text-primary"
                  >
                    <Icon className="size-4 text-muted-foreground" />
                    {category.name}
                  </Link>
                </li>
              );
            })}
          </ul>
        </nav>
        <BannerCarousel banners={banners} />
        <div className="hidden grid-rows-2 gap-4 lg:grid">
          <SideBanner
            href="/books?category=kinh-te"
            title="Sách kinh tế"
            note="Mua 2 giảm thêm 10%"
            className="bg-[oklch(0.93_0.04_160)]"
            book={bySlug("cha-giau-cha-ngheo")}
          />
          <SideBanner
            href="/register"
            title="Thành viên mới"
            note="Nhận mã giảm 50K"
            className="bg-secondary"
            book={bySlug("thoi-quen-nguyen-tu")}
          />
        </div>
      </section>

      {/* Quick categories (mobile & tablet) */}
      <nav aria-label="Danh mục" className="rounded-lg bg-card p-3 lg:hidden">
        <ul className="grid grid-cols-4 gap-2 sm:grid-cols-8">
          {categories.map((category) => {
            const Icon = categoryIcons[category.id];
            return (
              <li key={category.id}>
                <Link href={`/books?category=${category.slug}`} className="flex flex-col items-center gap-1.5 text-center">
                  <span className="grid size-11 place-items-center rounded-full bg-secondary text-secondary-foreground">
                    <Icon className="size-5" />
                  </span>
                  <span className="text-xs leading-tight">{category.name}</span>
                </Link>
              </li>
            );
          })}
        </ul>
      </nav>

      <FlashSale books={onSale} />

      <Ranking groups={rankingGroups} />

      <section aria-labelledby="suggest-heading">
        <div className="flex items-center justify-between rounded-t-lg border-b bg-card px-4 py-3">
          <h2 id="suggest-heading" className="text-lg font-bold">
            Gợi ý hôm nay
          </h2>
          <Link href="/books" className="flex items-center text-sm font-medium text-primary">
            Xem tất cả <ChevronRight className="size-4" />
          </Link>
        </div>
        <div className="grid grid-cols-2 gap-2 pt-2 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-6">
          {books.slice(0, 18).map((book) => (
            <BookCard key={book.id} book={book} />
          ))}
        </div>
        <div className="flex justify-center pt-4">
          <Link
            href="/books"
            className="rounded-md border border-primary bg-card px-10 py-2 text-sm font-semibold text-primary hover:bg-primary hover:text-primary-foreground"
          >
            Xem thêm
          </Link>
        </div>
      </section>
    </div>
  );
}

function SideBanner({
  href,
  title,
  note,
  className,
  book,
}: {
  href: string;
  title: string;
  note: string;
  className: string;
  book: (typeof books)[number];
}) {
  return (
    <Link href={href} className={`flex items-center justify-between gap-3 overflow-hidden rounded-lg px-5 ${className}`}>
      <div>
        <p className="font-bold">{title}</p>
        <p className="text-sm text-muted-foreground">{note}</p>
      </div>
      <div className="w-16 shrink-0 rotate-6">
        <BookCover book={book} size="sm" />
      </div>
    </Link>
  );
}
