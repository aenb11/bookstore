"use client";

import { useState } from "react";
import { Plus } from "lucide-react";
import { toast } from "sonner";
import { Button } from "@/components/ui/button";
import {
  Dialog,
  DialogClose,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/components/ui/dialog";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select";
import { Textarea } from "@/components/ui/textarea";
import { categories } from "@/lib/mock-data";

export function BookFormDialog() {
  const [open, setOpen] = useState(false);

  return (
    <Dialog open={open} onOpenChange={setOpen}>
      <DialogTrigger asChild>
        <Button>
          <Plus /> Thêm sách
        </Button>
      </DialogTrigger>
      <DialogContent className="sm:max-w-xl">
        <DialogHeader>
          <DialogTitle className="font-heading text-xl">Thêm sách mới</DialogTitle>
          <DialogDescription>Điền thông tin đầu sách. Các trường có dấu * là bắt buộc.</DialogDescription>
        </DialogHeader>
        <form
          id="book-form"
          className="grid gap-4 sm:grid-cols-2"
          onSubmit={(event) => {
            event.preventDefault();
            setOpen(false);
            toast.success("Đã thêm sách (dữ liệu mẫu, chưa lưu)");
          }}
        >
          <FormField id="title" label="Tên sách *" className="sm:col-span-2">
            <Input id="title" name="title" required />
          </FormField>
          <FormField id="author" label="Tác giả *">
            <Input id="author" name="author" required />
          </FormField>
          <FormField id="publisher" label="Nhà xuất bản">
            <Input id="publisher" name="publisher" />
          </FormField>
          <FormField id="category" label="Danh mục *">
            <Select name="category" required>
              <SelectTrigger id="category" className="w-full">
                <SelectValue placeholder="Chọn danh mục" />
              </SelectTrigger>
              <SelectContent>
                {categories.map((c) => (
                  <SelectItem key={c.id} value={c.id}>
                    {c.name}
                  </SelectItem>
                ))}
              </SelectContent>
            </Select>
          </FormField>
          <FormField id="isbn" label="ISBN">
            <Input id="isbn" name="isbn" placeholder="978-604-…" />
          </FormField>
          <FormField id="price" label="Giá bán (₫) *">
            <Input id="price" name="price" type="number" min={0} step={1000} required />
          </FormField>
          <FormField id="stock" label="Số lượng tồn *">
            <Input id="stock" name="stock" type="number" min={0} required />
          </FormField>
          <FormField id="description" label="Mô tả" className="sm:col-span-2">
            <Textarea id="description" name="description" rows={4} />
          </FormField>
        </form>
        <DialogFooter>
          <DialogClose asChild>
            <Button variant="outline">Hủy</Button>
          </DialogClose>
          <Button type="submit" form="book-form">
            Lưu sách
          </Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  );
}

function FormField({
  id,
  label,
  className,
  children,
}: {
  id: string;
  label: string;
  className?: string;
  children: React.ReactNode;
}) {
  return (
    <div className={className}>
      <Label htmlFor={id} className="mb-2">
        {label}
      </Label>
      {children}
    </div>
  );
}
