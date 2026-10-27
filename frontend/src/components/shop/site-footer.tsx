import Link from "next/link";
import { Mail, MapPin, Phone } from "lucide-react";
import { categories } from "@/lib/mock-data";
import { Logo } from "./logo";

const supportLinks = [
  { href: "#", label: "Hướng dẫn mua hàng" },
  { href: "#", label: "Chính sách đổi trả" },
  { href: "#", label: "Phương thức vận chuyển" },
  { href: "#", label: "Câu hỏi thường gặp" },
];

export function SiteFooter() {
  return (
    <footer className="mt-10 border-t bg-card">
      <div className="container-page grid gap-10 py-12 sm:grid-cols-2 lg:grid-cols-4">
        <div className="space-y-4">
          <Logo />
          <p className="max-w-xs text-sm leading-relaxed text-muted-foreground">
            Nhà sách trực tuyến dành cho người yêu chữ. Tuyển chọn cẩn thận, giao tận tay trên toàn quốc.
          </p>
        </div>
        <FooterColumn title="Danh mục">
          {categories.slice(0, 5).map((category) => (
            <li key={category.id}>
              <Link href={`/books?category=${category.slug}`} className="hover:text-foreground">
                {category.name}
              </Link>
            </li>
          ))}
        </FooterColumn>
        <FooterColumn title="Hỗ trợ">
          {supportLinks.map((link) => (
            <li key={link.label}>
              <Link href={link.href} className="hover:text-foreground">
                {link.label}
              </Link>
            </li>
          ))}
        </FooterColumn>
        <FooterColumn title="Liên hệ">
          <li className="flex gap-2">
            <MapPin className="mt-0.5 size-4 shrink-0" /> Khu phố 6, P. Linh Trung, TP. Thủ Đức, TP.HCM
          </li>
          <li className="flex gap-2">
            <Phone className="mt-0.5 size-4 shrink-0" /> 1900 1234
          </li>
          <li className="flex gap-2">
            <Mail className="mt-0.5 size-4 shrink-0" /> hotro@trangsach.vn
          </li>
        </FooterColumn>
      </div>
      <div className="border-t">
        <p className="container-page py-5 text-xs text-muted-foreground">
          © 2026 Trang Sách. Đồ án môn học — UIT.
        </p>
      </div>
    </footer>
  );
}

function FooterColumn({ title, children }: { title: string; children: React.ReactNode }) {
  return (
    <div>
      <h2 className="mb-4 font-sans text-sm font-semibold tracking-wider uppercase">{title}</h2>
      <ul className="space-y-2.5 text-sm text-muted-foreground">{children}</ul>
    </div>
  );
}
