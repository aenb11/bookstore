import { Suspense } from "react";
import type { Metadata } from "next";
import { Skeleton } from "@/components/ui/skeleton";
import { requireUser } from "@/lib/auth/dal";
import { CheckoutForm } from "./checkout-form";

export const metadata: Metadata = { title: "Thanh toán" };

export default function CheckoutPage() {
  return (
    <div className="container-page py-10">
      <h1 className="mb-8 text-3xl font-semibold sm:text-4xl">Thanh toán</h1>
      <Suspense fallback={<Skeleton className="h-96" />}>
        <UserCheckoutForm />
      </Suspense>
    </div>
  );
}

async function UserCheckoutForm() {
  const user = await requireUser("/checkout");
  return <CheckoutForm defaultName={user.name} defaultEmail={user.email} />;
}
