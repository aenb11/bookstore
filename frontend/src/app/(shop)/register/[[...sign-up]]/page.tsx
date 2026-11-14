import { Suspense } from "react";
import type { Metadata } from "next";
import { SignUp } from "@clerk/nextjs";
import { Skeleton } from "@/components/ui/skeleton";

export const metadata: Metadata = { title: "Đăng ký" };

/** Clerk's sign-up flow (full name, email/password or Google, then email verification). */
export default function RegisterPage() {
  return (
    <div className="container-page flex justify-center py-16">
      {/* Reads the URL (sub-steps like /login/factor-one), so it streams in. */}
      <Suspense fallback={<Skeleton className="h-[480px] w-full max-w-100 rounded-xl" />}>
        <SignUp path="/register" signInUrl="/login" />
      </Suspense>
    </div>
  );
}
