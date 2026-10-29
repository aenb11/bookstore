"use client";

import { usePathname, useRouter, useSearchParams } from "next/navigation";
import { useState } from "react";
import { SlidersHorizontal, Star } from "lucide-react";
import { Button } from "@/components/ui/button";
import { Checkbox } from "@/components/ui/checkbox";
import { Label } from "@/components/ui/label";
import { RadioGroup, RadioGroupItem } from "@/components/ui/radio-group";
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select";
import { Separator } from "@/components/ui/separator";
import { Sheet, SheetContent, SheetHeader, SheetTitle, SheetTrigger } from "@/components/ui/sheet";
import { Slider } from "@/components/ui/slider";
import { PRICE_MAX, sortOptions } from "@/lib/catalog";
import { formatVND } from "@/lib/format";
import { categories } from "@/lib/mock-data";

function useUpdateParams() {
  const router = useRouter();
  const pathname = usePathname();
  const searchParams = useSearchParams();

  return (updates: Record<string, string | null>) => {
    const params = new URLSearchParams(searchParams);
    for (const [key, value] of Object.entries(updates)) {
      if (value === null || value === "") params.delete(key);
      else params.set(key, value);
    }
    params.delete("page");
    const query = params.toString();
    router.push(query ? `${pathname}?${query}` : pathname, { scroll: false });
  };
}

function FilterPanel() {
  const searchParams = useSearchParams();
  const update = useUpdateParams();
  const activeCategory = searchParams.get("category") ?? "";
  const priceKey = `${searchParams.get("min")}-${searchParams.get("max")}`;

  return (
    <div className="space-y-6">
      <fieldset className="space-y-3">
        <legend className="mb-3 text-sm font-semibold">Danh mục</legend>
        <RadioGroup value={activeCategory} onValueChange={(value) => update({ category: value })}>
          <div className="flex items-center gap-2">
            <RadioGroupItem value="" id="cat-all" />
            <Label htmlFor="cat-all" className="font-normal">
              Tất cả
            </Label>
          </div>
          {categories.map((category) => (
            <div key={category.id} className="flex items-center gap-2">
              <RadioGroupItem value={category.slug} id={`cat-${category.slug}`} />
              <Label htmlFor={`cat-${category.slug}`} className="font-normal">
                {category.name}
              </Label>
            </div>
          ))}
        </RadioGroup>
      </fieldset>

      <Separator />

      {/* Keyed so the slider resets when the URL changes from elsewhere (e.g. "Xóa bộ lọc"). */}
      <PriceFilter
        key={priceKey}
        initial={[Number(searchParams.get("min")) || 0, Number(searchParams.get("max")) || PRICE_MAX]}
        onCommit={([min, max]) =>
          update({ min: min > 0 ? String(min) : null, max: max < PRICE_MAX ? String(max) : null })
        }
      />

      <Separator />

      <fieldset>
        <legend className="mb-3 text-sm font-semibold">Đánh giá</legend>
        <RadioGroup value={searchParams.get("rating") ?? ""} onValueChange={(value) => update({ rating: value })}>
          {["4.5", "4", ""].map((value) => (
            <div key={value} className="flex items-center gap-2">
              <RadioGroupItem value={value} id={`rating-${value || "all"}`} />
              <Label htmlFor={`rating-${value || "all"}`} className="font-normal">
                {value ? (
                  <>
                    Từ {value} <Star className="size-3.5 fill-rating text-rating" /> trở lên
                  </>
                ) : (
                  "Tất cả"
                )}
              </Label>
            </div>
          ))}
        </RadioGroup>
      </fieldset>

      <Separator />

      <fieldset className="space-y-3">
        <legend className="mb-3 text-sm font-semibold">Khác</legend>
        <div className="flex items-center gap-2">
          <Checkbox
            id="filter-sale"
            checked={searchParams.has("sale")}
            onCheckedChange={(checked) => update({ sale: checked ? "1" : null })}
          />
          <Label htmlFor="filter-sale" className="font-normal">
            Đang giảm giá
          </Label>
        </div>
        <div className="flex items-center gap-2">
          <Checkbox
            id="filter-stock"
            checked={searchParams.has("stock")}
            onCheckedChange={(checked) => update({ stock: checked ? "1" : null })}
          />
          <Label htmlFor="filter-stock" className="font-normal">
            Còn hàng
          </Label>
        </div>
      </fieldset>
    </div>
  );
}

function PriceFilter({ initial, onCommit }: { initial: number[]; onCommit: (value: number[]) => void }) {
  const [value, setValue] = useState(initial);
  return (
    <fieldset>
      <legend className="mb-4 text-sm font-semibold">Khoảng giá</legend>
      <Slider
        min={0}
        max={PRICE_MAX}
        step={10000}
        value={value}
        onValueChange={setValue}
        onValueCommit={onCommit}
        aria-label="Khoảng giá"
      />
      <div className="mt-3 flex justify-between text-xs text-muted-foreground">
        <span>{formatVND(value[0])}</span>
        <span>
          {formatVND(value[1])}
          {value[1] === PRICE_MAX && "+"}
        </span>
      </div>
    </fieldset>
  );
}

export function CatalogSidebar() {
  return (
    <aside aria-label="Bộ lọc" className="hidden w-60 shrink-0 lg:block">
      <div className="sticky top-32 rounded-lg bg-card p-4">
        <FilterPanel />
      </div>
    </aside>
  );
}

export function CatalogToolbar({ total }: { total: number }) {
  const searchParams = useSearchParams();
  const update = useUpdateParams();
  const router = useRouter();
  const pathname = usePathname();
  const hasFilters = [...searchParams.keys()].some((key) => key !== "sort" && key !== "page");

  return (
    <div className="flex flex-wrap items-center gap-3">
      <p className="mr-auto text-sm text-muted-foreground" aria-live="polite">
        {total} kết quả
      </p>
      {hasFilters && (
        <Button variant="link" size="sm" onClick={() => router.push(pathname)}>
          Xóa bộ lọc
        </Button>
      )}
      <Sheet>
        <SheetTrigger asChild>
          <Button variant="outline" className="lg:hidden">
            <SlidersHorizontal /> Bộ lọc
          </Button>
        </SheetTrigger>
        <SheetContent side="left" className="w-80">
          <SheetHeader>
            <SheetTitle className="font-heading text-xl">Bộ lọc</SheetTitle>
          </SheetHeader>
          <div className="overflow-y-auto px-4 pb-6">
            <FilterPanel />
          </div>
        </SheetContent>
      </Sheet>
      <Select value={searchParams.get("sort") ?? "popular"} onValueChange={(value) => update({ sort: value })}>
        <SelectTrigger className="w-44" aria-label="Sắp xếp">
          <SelectValue />
        </SelectTrigger>
        <SelectContent align="end">
          {sortOptions.map((option) => (
            <SelectItem key={option.value} value={option.value}>
              {option.label}
            </SelectItem>
          ))}
        </SelectContent>
      </Select>
    </div>
  );
}
