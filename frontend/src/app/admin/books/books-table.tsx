"use client";

import { useState } from "react";
import { MoreHorizontal, Pencil, Search, Trash2, Eye } from "lucide-react";
import Link from "next/link";
import { BookCover } from "@/components/book/book-cover";
import { Badge } from "@/components/ui/badge";
import { Button } from "@/components/ui/button";
import { Card } from "@/components/ui/card";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import { Input } from "@/components/ui/input";
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select";
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table";
import { formatVND } from "@/lib/format";
import { LOW_STOCK_THRESHOLD, books, categories, getCategory } from "@/lib/mock-data";

function StockBadge({ stock }: { stock: number }) {
  if (stock === 0) return <Badge variant="destructive">Hết hàng</Badge>;
  if (stock <= LOW_STOCK_THRESHOLD)
    return (
      <Badge variant="outline" className="border-transparent bg-amber-100 text-amber-900">
        Sắp hết · {stock}
      </Badge>
    );
  return <span className="tabular-nums">{stock}</span>;
}

export function BooksTable() {
  const [query, setQuery] = useState("");
  const [category, setCategory] = useState("all");

  const rows = books.filter(
    (book) =>
      (category === "all" || book.categoryId === category) &&
      `${book.title} ${book.author} ${book.isbn}`.toLowerCase().includes(query.trim().toLowerCase()),
  );

  return (
    <Card className="gap-0 py-0">
      <div className="flex flex-wrap gap-3 border-b p-4">
        <div className="relative w-full max-w-sm">
          <Search className="pointer-events-none absolute top-1/2 left-3 size-4 -translate-y-1/2 text-muted-foreground" />
          <Input
            value={query}
            onChange={(event) => setQuery(event.target.value)}
            placeholder="Tìm theo tên, tác giả, ISBN…"
            aria-label="Tìm sách"
            className="pl-9"
          />
        </div>
        <Select value={category} onValueChange={setCategory}>
          <SelectTrigger className="w-52" aria-label="Lọc theo danh mục">
            <SelectValue />
          </SelectTrigger>
          <SelectContent>
            <SelectItem value="all">Tất cả danh mục</SelectItem>
            {categories.map((c) => (
              <SelectItem key={c.id} value={c.id}>
                {c.name}
              </SelectItem>
            ))}
          </SelectContent>
        </Select>
        <p className="ml-auto self-center text-sm text-muted-foreground">{rows.length} đầu sách</p>
      </div>
      <Table>
        <TableHeader>
          <TableRow>
            <TableHead className="pl-4">Sách</TableHead>
            <TableHead className="hidden md:table-cell">Danh mục</TableHead>
            <TableHead className="text-right">Giá bán</TableHead>
            <TableHead className="hidden text-right sm:table-cell">Đã bán</TableHead>
            <TableHead>Tồn kho</TableHead>
            <TableHead className="w-12 pr-4">
              <span className="sr-only">Thao tác</span>
            </TableHead>
          </TableRow>
        </TableHeader>
        <TableBody>
          {rows.map((book) => (
            <TableRow key={book.id}>
              <TableCell className="pl-4">
                <div className="flex items-center gap-3">
                  <div className="w-9 shrink-0">
                    <BookCover book={book} size="xs" />
                  </div>
                  <div className="min-w-0">
                    <p className="max-w-64 truncate font-medium">{book.title}</p>
                    <p className="text-xs text-muted-foreground">{book.author}</p>
                  </div>
                </div>
              </TableCell>
              <TableCell className="hidden md:table-cell">{getCategory(book.categoryId)?.name}</TableCell>
              <TableCell className="text-right">{formatVND(book.price)}</TableCell>
              <TableCell className="hidden text-right tabular-nums sm:table-cell">
                {book.soldCount.toLocaleString("vi-VN")}
              </TableCell>
              <TableCell>
                <StockBadge stock={book.stock} />
              </TableCell>
              <TableCell className="pr-4">
                <DropdownMenu>
                  <DropdownMenuTrigger asChild>
                    <Button variant="ghost" size="icon-sm" aria-label={`Thao tác với ${book.title}`}>
                      <MoreHorizontal />
                    </Button>
                  </DropdownMenuTrigger>
                  <DropdownMenuContent align="end">
                    <DropdownMenuItem asChild>
                      <Link href={`/books/${book.slug}`}>
                        <Eye /> Xem trên cửa hàng
                      </Link>
                    </DropdownMenuItem>
                    <DropdownMenuItem>
                      <Pencil /> Chỉnh sửa
                    </DropdownMenuItem>
                    <DropdownMenuSeparator />
                    <DropdownMenuItem variant="destructive">
                      <Trash2 /> Xóa
                    </DropdownMenuItem>
                  </DropdownMenuContent>
                </DropdownMenu>
              </TableCell>
            </TableRow>
          ))}
          {rows.length === 0 && (
            <TableRow>
              <TableCell colSpan={6} className="py-12 text-center text-muted-foreground">
                Không có sách nào khớp với bộ lọc.
              </TableCell>
            </TableRow>
          )}
        </TableBody>
      </Table>
    </Card>
  );
}
