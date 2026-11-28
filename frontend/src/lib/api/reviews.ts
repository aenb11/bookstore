import type { Review } from "@/lib/types";

type ApiReview = {
  id: number;
  rating: number;
  comment: string | null;
  accountFullName: string;
  createdAt: string;
};

type PagedResult<T> = {
  items: T[];
};

const apiUrl = process.env.BOOKSTORE_API_URL ?? "http://localhost:5187";

export async function getBookReviews(bookId: number): Promise<Review[]> {
  const response = await fetch(
    `${apiUrl}/api/v1/reviews/book/${bookId}?pageNumber=1&pageSize=50`,
    { cache: "no-store" },
  );

  if (!response.ok) {
    throw new Error(`Unable to load reviews for book ${bookId}: ${response.status} ${response.statusText}`);
  }

  const result: PagedResult<ApiReview> = await response.json();

  return result.items.map((review) => ({
    id: String(review.id),
    author: review.accountFullName,
    rating: review.rating,
    content: review.comment ?? "Người dùng không để lại nhận xét.",
    createdAt: review.createdAt,
  }));
}
