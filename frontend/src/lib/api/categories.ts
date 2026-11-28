export type ApiCategory = {
  id: number;
  name: string;
};

type PagedResult<T> = {
  items: T[];
  totalPages: number;
};

const apiUrl = process.env.BOOKSTORE_API_URL ?? "http://localhost:5187";

export async function getCategories(): Promise<ApiCategory[]> {
  const categories: ApiCategory[] = [];
  let totalPages = 1;

  for (let pageNumber = 1; pageNumber <= totalPages; pageNumber++) {
    const response = await fetch(
      `${apiUrl}/api/v1/categories?pageNumber=${pageNumber}&pageSize=50`,
      { cache: "no-store" },
    );

    if (!response.ok) {
      throw new Error(`Unable to load categories: ${response.status} ${response.statusText}`);
    }

    const result: PagedResult<ApiCategory> = await response.json();
    categories.push(...result.items);
    totalPages = result.totalPages;
  }

  return categories;
}
