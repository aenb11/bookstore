"use client";

import { useSyncExternalStore } from "react";

export type CartItem = { bookId: string; quantity: number };

const STORAGE_KEY = "trang-sach:cart";
const EMPTY: CartItem[] = [];

let items: CartItem[] = EMPTY;
let loaded = false;
const listeners = new Set<() => void>();

function load() {
  if (loaded) return;
  loaded = true;
  try {
    const raw = window.localStorage.getItem(STORAGE_KEY);
    if (raw) items = JSON.parse(raw) as CartItem[];
  } catch {
    items = EMPTY;
  }
}

function commit(next: CartItem[]) {
  items = next;
  window.localStorage.setItem(STORAGE_KEY, JSON.stringify(next));
  listeners.forEach((listener) => listener());
}

function subscribe(listener: () => void) {
  listeners.add(listener);
  const onStorage = (event: StorageEvent) => {
    if (event.key !== STORAGE_KEY) return;
    loaded = false;
    load();
    listener();
  };
  window.addEventListener("storage", onStorage);
  return () => {
    listeners.delete(listener);
    window.removeEventListener("storage", onStorage);
  };
}

function getSnapshot() {
  load();
  return items;
}

export const cart = {
  add(bookId: string, quantity = 1) {
    load();
    const existing = items.find((item) => item.bookId === bookId);
    commit(
      existing
        ? items.map((item) =>
            item.bookId === bookId ? { ...item, quantity: item.quantity + quantity } : item,
          )
        : [...items, { bookId, quantity }],
    );
  },
  setQuantity(bookId: string, quantity: number) {
    load();
    commit(
      quantity <= 0
        ? items.filter((item) => item.bookId !== bookId)
        : items.map((item) => (item.bookId === bookId ? { ...item, quantity } : item)),
    );
  },
  remove(bookId: string) {
    load();
    commit(items.filter((item) => item.bookId !== bookId));
  },
  clear() {
    commit(EMPTY);
  },
};

export function useCart() {
  return useSyncExternalStore(subscribe, getSnapshot, () => EMPTY);
}
