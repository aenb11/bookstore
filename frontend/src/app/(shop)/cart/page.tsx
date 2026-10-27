import type { Metadata } from "next";
import { CartView } from "./cart-view";

export const metadata: Metadata = { title: "Giỏ hàng" };

export default function CartPage() {
  return (
    <div className="container-page py-10">
      <h1 className="mb-8 text-3xl font-semibold sm:text-4xl">Giỏ hàng</h1>
      <CartView />
    </div>
  );
}
