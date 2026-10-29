"use client";

import Link from "next/link";
import { useRouter } from "next/navigation";
import { Banknote, Landmark, Wallet } from "lucide-react";
import { BookCover } from "@/components/book/book-cover";
import { useCartLines } from "@/components/shop/cart-sheet";
import { OrderSummary } from "@/components/shop/order-summary";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { RadioGroup, RadioGroupItem } from "@/components/ui/radio-group";
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select";
import { Textarea } from "@/components/ui/textarea";
import { formatVND } from "@/lib/format";

const provinces = ["TP. Hồ Chí Minh", "Hà Nội", "Đà Nẵng", "Hải Phòng", "Cần Thơ", "Bình Dương", "Đồng Nai", "Khánh Hòa"];

const paymentMethods = [
  { value: "cod", icon: Banknote, label: "Thanh toán khi nhận hàng (COD)", hint: "Trả tiền mặt cho nhân viên giao hàng" },
  { value: "bank", icon: Landmark, label: "Chuyển khoản ngân hàng", hint: "Quét mã VietQR sau khi đặt hàng" },
  { value: "ewallet", icon: Wallet, label: "Ví điện tử", hint: "MoMo, ZaloPay, VNPay" },
];

export function CheckoutForm() {
  const { lines, subtotal } = useCartLines();
  const router = useRouter();

  if (lines.length === 0) {
    return (
      <div className="rounded-xl border border-dashed py-16 text-center">
        <p className="mb-4 text-muted-foreground">Giỏ hàng trống, chưa có gì để thanh toán.</p>
        <Button asChild>
          <Link href="/books">Khám phá sách</Link>
        </Button>
      </div>
    );
  }

  return (
    <form
      className="grid gap-8 lg:grid-cols-[1fr_400px]"
      onSubmit={(event) => {
        event.preventDefault();
        router.push("/checkout/success");
      }}
    >
      <div className="space-y-6">
        <Card>
          <CardHeader>
            <CardTitle className="font-heading text-xl">1. Thông tin giao hàng</CardTitle>
            <CardDescription>Chúng tôi sẽ liên hệ qua số điện thoại này khi giao hàng.</CardDescription>
          </CardHeader>
          <CardContent className="grid gap-4 sm:grid-cols-2">
            <Field id="name" label="Họ và tên">
              <Input id="name" name="name" autoComplete="name" required defaultValue="Nguyễn Minh Anh" />
            </Field>
            <Field id="phone" label="Số điện thoại">
              <Input id="phone" name="phone" type="tel" autoComplete="tel" required placeholder="09xx xxx xxx" />
            </Field>
            <Field id="email" label="Email" className="sm:col-span-2">
              <Input id="email" name="email" type="email" autoComplete="email" placeholder="email@cuaban.com" />
            </Field>
            <Field id="province" label="Tỉnh / Thành phố">
              <Select name="province" defaultValue={provinces[0]} required>
                <SelectTrigger id="province" className="w-full">
                  <SelectValue />
                </SelectTrigger>
                <SelectContent>
                  {provinces.map((province) => (
                    <SelectItem key={province} value={province}>
                      {province}
                    </SelectItem>
                  ))}
                </SelectContent>
              </Select>
            </Field>
            <Field id="ward" label="Phường / Xã">
              <Input id="ward" name="ward" required placeholder="Phường Linh Trung" />
            </Field>
            <Field id="address" label="Địa chỉ cụ thể" className="sm:col-span-2">
              <Input id="address" name="address" autoComplete="street-address" required placeholder="Số nhà, tên đường" />
            </Field>
            <Field id="note" label="Ghi chú (không bắt buộc)" className="sm:col-span-2">
              <Textarea id="note" name="note" placeholder="Ví dụ: giao giờ hành chính, gói quà…" />
            </Field>
          </CardContent>
        </Card>

        <Card>
          <CardHeader>
            <CardTitle className="font-heading text-xl">2. Phương thức thanh toán</CardTitle>
          </CardHeader>
          <CardContent>
            <RadioGroup name="payment" defaultValue="cod" className="gap-3">
              {paymentMethods.map(({ value, icon: Icon, label, hint }) => (
                <Label
                  key={value}
                  htmlFor={`pay-${value}`}
                  className="flex cursor-pointer items-center gap-4 rounded-lg border p-4 font-normal transition-colors hover:bg-muted/50 has-data-[state=checked]:border-primary has-data-[state=checked]:bg-primary/5"
                >
                  <RadioGroupItem value={value} id={`pay-${value}`} />
                  <Icon className="size-5 text-primary" />
                  <span className="flex flex-col gap-0.5">
                    <span className="font-medium">{label}</span>
                    <span className="text-xs text-muted-foreground">{hint}</span>
                  </span>
                </Label>
              ))}
            </RadioGroup>
          </CardContent>
        </Card>
      </div>

      <Card className="h-fit lg:sticky lg:top-32">
        <CardHeader>
          <CardTitle className="font-heading text-xl">Đơn hàng ({lines.length})</CardTitle>
        </CardHeader>
        <CardContent className="space-y-5">
          <ul className="max-h-72 space-y-3 overflow-y-auto">
            {lines.map(({ book, quantity }) => (
              <li key={book.id} className="flex items-center gap-3">
                <div className="relative w-12 shrink-0">
                  <BookCover book={book} size="xs" />
                  <span className="absolute -top-1.5 -right-1.5 grid size-5 place-items-center rounded-full bg-foreground text-[0.65rem] text-background">
                    {quantity}
                  </span>
                </div>
                <p className="line-clamp-2 flex-1 text-sm">{book.title}</p>
                <span className="text-sm font-medium">{formatVND(book.price * quantity)}</span>
              </li>
            ))}
          </ul>
          <OrderSummary subtotal={subtotal} />
          <Button type="submit" size="lg" className="h-11 w-full">
            Đặt hàng
          </Button>
          <p className="text-center text-xs text-muted-foreground">
            Bằng việc đặt hàng, bạn đồng ý với điều khoản sử dụng của Trang Sách.
          </p>
        </CardContent>
      </Card>
    </form>
  );
}

function Field({
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
