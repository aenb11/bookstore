import type { Metadata } from "next";
import { AuthCard, AuthLink } from "@/components/shop/auth-card";

export const metadata: Metadata = { title: "Đăng ký" };

export default function RegisterPage() {
  return (
    <AuthCard
      title="Tạo tài khoản"
      description="Nhận mã giảm 10% cho đơn hàng đầu tiên khi đăng ký thành viên."
      fields={[
        { id: "name", label: "Họ và tên", autoComplete: "name", placeholder: "Nguyễn Văn A" },
        { id: "email", label: "Email", type: "email", autoComplete: "email", placeholder: "email@cuaban.com" },
        { id: "phone", label: "Số điện thoại", type: "tel", autoComplete: "tel", placeholder: "09xx xxx xxx" },
        { id: "password", label: "Mật khẩu", type: "password", autoComplete: "new-password" },
      ]}
      submitLabel="Đăng ký"
      footer={
        <>
          Đã có tài khoản?<AuthLink href="/login">Đăng nhập</AuthLink>
        </>
      }
    />
  );
}
