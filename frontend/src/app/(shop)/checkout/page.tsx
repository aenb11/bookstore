import type { Metadata } from "next";
import { CheckoutForm } from "./checkout-form";

export const metadata: Metadata = { title: "Thanh toán" };

export default function CheckoutPage() {
  return (
    <div className="container-page py-10">
      <h1 className="mb-8 text-3xl font-semibold sm:text-4xl">Thanh toán</h1>
      <CheckoutForm />
    </div>
  );
}
