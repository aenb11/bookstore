import { Suspense } from "react";
import type { Metadata } from "next";
import { SignIn } from "@clerk/nextjs";
import { Skeleton } from "@/components/ui/skeleton";

export const metadata: Metadata = { title: "Đăng nhập" };

/** Clerk's sign-in flow (email/password, Google, forgot password, verification steps). */
export default function LoginPage() {
  return (
    <div className="container-page flex justify-center py-16">
      {/* Reads the URL (sub-steps like /login/factor-one), so it streams in. */}
      <Suspense fallback={<Skeleton className="h-[480px] w-full max-w-100 rounded-xl" />}>
        <SignIn path="/login" signUpUrl="/register" />
      </Suspense>
    </div>
  );
}
