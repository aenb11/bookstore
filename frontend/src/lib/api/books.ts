export type ApiBookDetail = {
  id: number;
  isbn: string | null;
  title: string;
  category: { id: number; name: string };
  serie: { id: number; name: string; description: string | null } | null;
  publisher: { id: number; name: string };
  publicationYear: number;
  language: string;
  pageCount: number;
  coverType: string;
  weight: number;
  ageGroup: string;
  listPrice: number;
  salePrice: number;
  stockQuantity: number;
  description: string | null;
  totalReviews: number;
  averageRating: number;
  soldCount: number;
  authors: { id: number; fullName: string; role: string }[];
  images: { url: string; sortOrder: number }[];
  tags: { name: string }[];
};

const apiUrl = process.env.BOOKSTORE_API_URL ?? "http://localhost:5187";

export async function getBookDetail(id: number): Promise<ApiBookDetail | null> {
  const response = await fetch(`${apiUrl}/api/v1/books/${id}`, { cache: "no-store" });

  if (response.status === 404) return null;
  if (!response.ok) {
    throw new Error(`Unable to load book ${id}: ${response.status} ${response.statusText}`);
  }

  return response.json();
}
