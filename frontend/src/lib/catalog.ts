import { books, categories } from "./mock-data";
import type { Book } from "./types";

export const PAGE_SIZE = 12;
export const PRICE_MAX = 300000;

export const sortOptions = [
  { value: "popular", label: "Phổ biến" },
  { value: "bestselling", label: "Bán chạy" },
  { value: "newest", label: "Mới nhất" },
  { value: "price-asc", label: "Giá thấp đến cao" },
  { value: "price-desc", label: "Giá cao đến thấp" },
] as const;

export type CatalogParams = {
  q?: string;
  category?: string;
  sort?: string;
  sale?: string;
  min?: string;
  max?: string;
  rating?: string;
  stock?: string;
  page?: string;
};

function normalize(text: string) {
  return text.normalize("NFD").replace(/[̀-ͯ]/g, "").replace(/đ/gi, "d").toLowerCase();
}

const sorters: Record<string, (a: Book, b: Book) => number> = {
  popular: (a, b) => b.rating * b.reviewCount - a.rating * a.reviewCount,
  bestselling: (a, b) => b.soldCount - a.soldCount,
  newest: (a, b) => Number(Boolean(b.isNew)) - Number(Boolean(a.isNew)) || b.publishedYear - a.publishedYear,
  "price-asc": (a, b) => a.price - b.price,
  "price-desc": (a, b) => b.price - a.price,
};

export function queryCatalog(params: CatalogParams) {
  const category = categories.find((c) => c.slug === params.category);
  const q = params.q ? normalize(params.q.trim()) : "";
  const min = Number(params.min) || 0;
  const max = Number(params.max) || Infinity;
  const minRating = Number(params.rating) || 0;

  const filtered = books
    .filter((book) => !category || book.categoryId === category.id)
    .filter((book) => !q || normalize(`${book.title} ${book.author}`).includes(q))
    .filter((book) => book.price >= min && book.price <= max)
    .filter((book) => book.rating >= minRating)
    .filter((book) => !params.sale || Boolean(book.originalPrice))
    .filter((book) => !params.stock || book.stock > 0)
    .sort(sorters[params.sort ?? "popular"] ?? sorters.popular);

  const totalPages = Math.max(1, Math.ceil(filtered.length / PAGE_SIZE));
  const page = Math.min(Math.max(Number(params.page) || 1, 1), totalPages);

  return {
    category,
    total: filtered.length,
    page,
    totalPages,
    items: filtered.slice((page - 1) * PAGE_SIZE, page * PAGE_SIZE),
  };
}
