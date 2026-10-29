"use client";

import { useEffect } from "react";
import { cart } from "@/lib/cart-store";

/** Empties the cart once the order is placed (done here to avoid flashing an empty checkout). */
export function ClearCart() {
  useEffect(() => cart.clear(), []);
  return null;
}
