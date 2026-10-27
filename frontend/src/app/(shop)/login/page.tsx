import type { Metadata } from "next";
import Link from "next/link";
import { AuthCard, AuthLink } from "@/components/shop/auth-card";

export const metadata: Metadata = { title: "Đăng nhập" };

export default function LoginPage() {
  return (
    <AuthCard
      title="Chào mừng trở lại"
      description="Đăng nhập để theo dõi đơn hàng và nhận ưu đãi dành riêng cho bạn."
      fields={[
        { id: "email", label: "Email", type: "email", autoComplete: "email", placeholder: "email@cuaban.com" },
        { id: "password", label: "Mật khẩu", type: "password", autoComplete: "current-password" },
      ]}
      extra={
        <div className="text-right">
          <Link href="#" className="text-sm text-primary hover:underline">
            Quên mật khẩu?
          </Link>
        </div>
      }
      submitLabel="Đăng nhập"
      footer={
        <>
          Chưa có tài khoản?<AuthLink href="/register">Đăng ký ngay</AuthLink>
        </>
      }
    />
  );
}
