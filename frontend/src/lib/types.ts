export type Category = {
  id: string;
  slug: string;
  name: string;
  description: string;
};

export type Review = {
  id: string;
  author: string;
  rating: number;
  content: string;
  createdAt: string;
};

export type Book = {
  id: string;
  slug: string;
  title: string;
  author: string;
  translator?: string;
  publisher: string;
  publishedYear: number;
  pages: number;
  categoryId: string;
  /** Price in VND. */
  price: number;
  /** Price before discount in VND, when on sale. */
  originalPrice?: number;
  rating: number;
  reviewCount: number;
  soldCount: number;
  stock: number;
  isNew?: boolean;
  description: string;
  isbn: string;
};

export type OrderStatus = "pending" | "shipping" | "completed" | "cancelled";

export type OrderItem = {
  bookId: string;
  quantity: number;
  unitPrice: number;
};

export type Order = {
  id: string;
  code: string;
  customerId: string;
  items: OrderItem[];
  shippingFee: number;
  status: OrderStatus;
  paymentMethod: "cod" | "bank" | "ewallet";
  shippingAddress: string;
  createdAt: string;
};

export type Customer = {
  id: string;
  name: string;
  email: string;
  phone: string;
  city: string;
  joinedAt: string;
};
