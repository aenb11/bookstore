const vndFormatter = new Intl.NumberFormat("vi-VN", {
  style: "currency",
  currency: "VND",
  maximumFractionDigits: 0,
});

const dateFormatter = new Intl.DateTimeFormat("vi-VN", {
  day: "2-digit",
  month: "2-digit",
  year: "numeric",
});

const compactFormatter = new Intl.NumberFormat("vi-VN", {
  notation: "compact",
  maximumFractionDigits: 1,
});

export function formatVND(amount: number) {
  return vndFormatter.format(amount);
}

export function formatDate(iso: string) {
  return dateFormatter.format(new Date(iso));
}

export function formatCompact(value: number) {
  return compactFormatter.format(value);
}

export function discountPercent(price: number, originalPrice?: number) {
  if (!originalPrice || originalPrice <= price) return 0;
  return Math.round(((originalPrice - price) / originalPrice) * 100);
}
