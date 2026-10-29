import type { Book, Category, Customer, Order, OrderStatus, Review } from "./types";

export const categories: Category[] = [
  { id: "c1", slug: "van-hoc-viet-nam", name: "Văn học Việt Nam", description: "Tiểu thuyết, truyện ngắn và thơ của các tác giả Việt." },
  { id: "c2", slug: "van-hoc-nuoc-ngoai", name: "Văn học nước ngoài", description: "Những tác phẩm kinh điển và đương đại được dịch sang tiếng Việt." },
  { id: "c3", slug: "kinh-te", name: "Kinh tế", description: "Quản trị, tài chính, khởi nghiệp và đầu tư." },
  { id: "c4", slug: "ky-nang-song", name: "Kỹ năng sống", description: "Phát triển bản thân, tư duy và thói quen." },
  { id: "c5", slug: "thieu-nhi", name: "Thiếu nhi", description: "Truyện tranh, truyện cổ tích và sách cho bé." },
  { id: "c6", slug: "lich-su", name: "Lịch sử", description: "Lịch sử Việt Nam và thế giới." },
  { id: "c7", slug: "khoa-hoc", name: "Khoa học", description: "Khoa học phổ thông, vũ trụ và công nghệ." },
  { id: "c8", slug: "tam-ly", name: "Tâm lý học", description: "Hiểu con người và hành vi." },
];

type BookSeed = Omit<Book, "id" | "slug" | "description" | "isbn" | "reviewCount"> & {
  description?: string;
};

const seeds: BookSeed[] = [
  { title: "Tôi thấy hoa vàng trên cỏ xanh", author: "Nguyễn Nhật Ánh", publisher: "NXB Trẻ", publishedYear: 2018, pages: 378, categoryId: "c1", price: 125000, originalPrice: 150000, rating: 4.8, soldCount: 12840, stock: 120 },
  { title: "Mắt biếc", author: "Nguyễn Nhật Ánh", publisher: "NXB Trẻ", publishedYear: 2019, pages: 300, categoryId: "c1", price: 99000, rating: 4.7, soldCount: 9650, stock: 85 },
  { title: "Số đỏ", author: "Vũ Trọng Phụng", publisher: "NXB Văn Học", publishedYear: 2020, pages: 268, categoryId: "c1", price: 78000, rating: 4.5, soldCount: 3420, stock: 42 },
  { title: "Nỗi buồn chiến tranh", author: "Bảo Ninh", publisher: "NXB Trẻ", publishedYear: 2021, pages: 340, categoryId: "c1", price: 115000, rating: 4.6, soldCount: 2150, stock: 6, isNew: true },
  { title: "Dế Mèn phiêu lưu ký", author: "Tô Hoài", publisher: "NXB Kim Đồng", publishedYear: 2022, pages: 156, categoryId: "c5", price: 55000, originalPrice: 68000, rating: 4.9, soldCount: 15230, stock: 200 },
  { title: "Nhà giả kim", author: "Paulo Coelho", translator: "Lê Chu Cầu", publisher: "NXB Hội Nhà Văn", publishedYear: 2020, pages: 228, categoryId: "c2", price: 79000, originalPrice: 99000, rating: 4.7, soldCount: 21340, stock: 150 },
  { title: "Rừng Na Uy", author: "Haruki Murakami", translator: "Trịnh Lữ", publisher: "NXB Hội Nhà Văn", publishedYear: 2021, pages: 556, categoryId: "c2", price: 158000, rating: 4.5, soldCount: 7820, stock: 64 },
  { title: "Trăm năm cô đơn", author: "Gabriel García Márquez", translator: "Nguyễn Trung Đức", publisher: "NXB Văn Học", publishedYear: 2019, pages: 520, categoryId: "c2", price: 165000, rating: 4.6, soldCount: 5410, stock: 31 },
  { title: "Hoàng tử bé", author: "Antoine de Saint-Exupéry", translator: "Trác Phong", publisher: "NXB Hội Nhà Văn", publishedYear: 2022, pages: 112, categoryId: "c5", price: 65000, rating: 4.9, soldCount: 18760, stock: 3, isNew: true },
  { title: "1984", author: "George Orwell", translator: "Đặng Phương Nghi", publisher: "NXB Hội Nhà Văn", publishedYear: 2023, pages: 412, categoryId: "c2", price: 135000, originalPrice: 160000, rating: 4.7, soldCount: 6230, stock: 58, isNew: true },
  { title: "Cha giàu cha nghèo", author: "Robert T. Kiyosaki", translator: "Thiên Kim", publisher: "NXB Trẻ", publishedYear: 2021, pages: 336, categoryId: "c3", price: 109000, rating: 4.4, soldCount: 19870, stock: 90 },
  { title: "Từ tốt đến vĩ đại", author: "Jim Collins", translator: "Trần Thị Ngân Tuyến", publisher: "NXB Trẻ", publishedYear: 2020, pages: 456, categoryId: "c3", price: 189000, originalPrice: 225000, rating: 4.6, soldCount: 4120, stock: 27 },
  { title: "Nghĩ giàu và làm giàu", author: "Napoleon Hill", translator: "Việt Khương", publisher: "NXB Tổng Hợp TP.HCM", publishedYear: 2019, pages: 384, categoryId: "c3", price: 98000, rating: 4.3, soldCount: 11200, stock: 110 },
  { title: "Đắc nhân tâm", author: "Dale Carnegie", translator: "Nguyễn Văn Phước", publisher: "NXB Tổng Hợp TP.HCM", publishedYear: 2022, pages: 320, categoryId: "c4", price: 86000, originalPrice: 108000, rating: 4.8, soldCount: 35420, stock: 240 },
  { title: "Thói quen nguyên tử", author: "James Clear", translator: "Vũ Phi Yên", publisher: "NXB Thế Giới", publishedYear: 2023, pages: 368, categoryId: "c4", price: 159000, rating: 4.8, soldCount: 14560, stock: 75, isNew: true },
  { title: "Tuổi trẻ đáng giá bao nhiêu", author: "Rosie Nguyễn", publisher: "NXB Hội Nhà Văn", publishedYear: 2018, pages: 285, categoryId: "c4", price: 90000, rating: 4.3, soldCount: 16740, stock: 4 },
  { title: "Doraemon — Tập 1", author: "Fujiko F. Fujio", publisher: "NXB Kim Đồng", publishedYear: 2023, pages: 192, categoryId: "c5", price: 25000, rating: 4.9, soldCount: 42100, stock: 500 },
  { title: "Sapiens: Lược sử loài người", author: "Yuval Noah Harari", translator: "Nguyễn Thủy Chung", publisher: "NXB Tri Thức", publishedYear: 2021, pages: 560, categoryId: "c6", price: 239000, originalPrice: 299000, rating: 4.7, soldCount: 8930, stock: 46 },
  { title: "Việt Nam sử lược", author: "Trần Trọng Kim", publisher: "NXB Văn Học", publishedYear: 2020, pages: 620, categoryId: "c6", price: 175000, rating: 4.5, soldCount: 3780, stock: 22 },
  { title: "Súng, vi trùng và thép", author: "Jared Diamond", translator: "Trần Tiễn Cao Đăng", publisher: "NXB Thế Giới", publishedYear: 2022, pages: 640, categoryId: "c6", price: 255000, rating: 4.6, soldCount: 2640, stock: 18 },
  { title: "Lược sử thời gian", author: "Stephen Hawking", translator: "Cao Chi", publisher: "NXB Trẻ", publishedYear: 2021, pages: 260, categoryId: "c7", price: 115000, rating: 4.7, soldCount: 6150, stock: 38 },
  { title: "Vũ trụ", author: "Carl Sagan", translator: "Nguyễn Viết Long", publisher: "NXB Thế Giới", publishedYear: 2023, pages: 488, categoryId: "c7", price: 229000, originalPrice: 268000, rating: 4.8, soldCount: 1980, stock: 2, isNew: true },
  { title: "Tư duy nhanh và chậm", author: "Daniel Kahneman", translator: "Hương Lan, Xuân Thanh", publisher: "NXB Thế Giới", publishedYear: 2022, pages: 612, categoryId: "c8", price: 219000, rating: 4.6, soldCount: 7340, stock: 52 },
  { title: "Đi tìm lẽ sống", author: "Viktor E. Frankl", translator: "Nguyễn Văn Bé", publisher: "NXB Tổng Hợp TP.HCM", publishedYear: 2021, pages: 224, categoryId: "c8", price: 88000, rating: 4.8, soldCount: 13450, stock: 67 },
];

function slugify(text: string) {
  return text
    .normalize("NFD")
    .replace(/[̀-ͯ]/g, "")
    .replace(/đ/g, "d")
    .replace(/Đ/g, "D")
    .toLowerCase()
    .replace(/[^a-z0-9]+/g, "-")
    .replace(/(^-|-$)/g, "");
}

export const books: Book[] = seeds.map((seed, index) => {
  const category = categories.find((c) => c.id === seed.categoryId)!;
  return {
    ...seed,
    id: `b${index + 1}`,
    slug: slugify(seed.title),
    isbn: `978-604-${String(1000 + index * 37).padStart(4, "0")}-${index % 10}`,
    reviewCount: Math.round(seed.soldCount / 45),
    description:
      seed.description ??
      `“${seed.title}” của ${seed.author} là một trong những cuốn sách được độc giả yêu thích nhất ở mảng ${category.name.toLowerCase()}. ` +
        `Với lối viết cuốn hút và giàu chiều sâu, tác phẩm đưa người đọc qua những trang sách đầy cảm xúc và suy ngẫm. ` +
        `Ấn bản ${seed.publishedYear} của ${seed.publisher} được trình bày trang nhã, giấy tốt, phù hợp để đọc và làm quà tặng.`,
  };
});

export const reviews: Review[] = [
  { id: "r1", author: "Minh Anh", rating: 5, content: "Sách đóng gói cẩn thận, giấy đẹp. Nội dung thì khỏi bàn, đọc một mạch không dừng được.", createdAt: "2026-10-12" },
  { id: "r2", author: "Quốc Bảo", rating: 4, content: "Giao hàng nhanh, giá tốt hơn ngoài nhà sách. Bìa hơi cong một chút nhưng không đáng kể.", createdAt: "2026-10-05" },
  { id: "r3", author: "Thu Hà", rating: 5, content: "Mua làm quà cho em gái, bé rất thích. Sẽ ủng hộ shop dài dài.", createdAt: "2026-09-28" },
];

export const customers: Customer[] = [
  { id: "u1", name: "Nguyễn Minh Anh", email: "minhanh@example.com", phone: "0901 234 567", city: "TP. Hồ Chí Minh", joinedAt: "2025-03-14" },
  { id: "u2", name: "Trần Quốc Bảo", email: "quocbao@example.com", phone: "0912 345 678", city: "Hà Nội", joinedAt: "2025-06-02" },
  { id: "u3", name: "Lê Thu Hà", email: "thuha@example.com", phone: "0938 456 789", city: "Đà Nẵng", joinedAt: "2025-08-21" },
  { id: "u4", name: "Phạm Gia Huy", email: "giahuy@example.com", phone: "0977 567 890", city: "Cần Thơ", joinedAt: "2026-01-09" },
  { id: "u5", name: "Võ Ngọc Linh", email: "ngoclinh@example.com", phone: "0965 678 901", city: "TP. Hồ Chí Minh", joinedAt: "2026-04-30" },
  { id: "u6", name: "Đặng Hoàng Nam", email: "hoangnam@example.com", phone: "0889 789 012", city: "Hải Phòng", joinedAt: "2026-09-17" },
];

const statuses: OrderStatus[] = ["pending", "shipping", "completed", "completed", "cancelled", "shipping", "completed", "pending"];

export const orders: Order[] = Array.from({ length: 16 }, (_, i) => {
  const customer = customers[i % customers.length];
  const items = [books[(i * 5) % books.length], books[(i * 7 + 3) % books.length]]
    .slice(0, (i % 2) + 1)
    .map((book, j) => ({ bookId: book.id, quantity: ((i + j) % 3) + 1, unitPrice: book.price }));
  const day = String(26 - i).padStart(2, "0");
  return {
    id: `o${i + 1}`,
    code: `TS${String(240150 - i)}`,
    customerId: customer.id,
    items,
    shippingFee: i % 3 === 0 ? 0 : 25000,
    status: statuses[i % statuses.length],
    paymentMethod: (["cod", "bank", "ewallet"] as const)[i % 3],
    shippingAddress: `${12 + i} Nguyễn Văn Cừ, ${customer.city}`,
    createdAt: `2026-10-${day}T${String(8 + (i % 12)).padStart(2, "0")}:15:00+07:00`,
  };
});

export const monthlyRevenue = [
  { month: "T5", revenue: 182_000_000, orders: 1240 },
  { month: "T6", revenue: 205_000_000, orders: 1385 },
  { month: "T7", revenue: 198_000_000, orders: 1310 },
  { month: "T8", revenue: 236_000_000, orders: 1562 },
  { month: "T9", revenue: 251_000_000, orders: 1648 },
  { month: "T10", revenue: 288_000_000, orders: 1873 },
];

export const LOW_STOCK_THRESHOLD = 10;
export const FREE_SHIPPING_THRESHOLD = 300000;
export const SHIPPING_FEE = 25000;

export function getBookBySlug(slug: string) {
  return books.find((book) => book.slug === slug);
}

export function getBookById(id: string) {
  return books.find((book) => book.id === id);
}

export function getCategory(id: string) {
  return categories.find((category) => category.id === id);
}

export function getCustomer(id: string) {
  return customers.find((customer) => customer.id === id);
}

export function orderTotal(order: Order) {
  return order.items.reduce((sum, item) => sum + item.unitPrice * item.quantity, 0) + order.shippingFee;
}

export function shippingFeeFor(subtotal: number) {
  return subtotal === 0 || subtotal >= FREE_SHIPPING_THRESHOLD ? 0 : SHIPPING_FEE;
}

export const orderStatusLabel: Record<OrderStatus, string> = {
  pending: "Chờ xử lý",
  shipping: "Đang giao",
  completed: "Hoàn thành",
  cancelled: "Đã hủy",
};

export const paymentMethodLabel: Record<Order["paymentMethod"], string> = {
  cod: "Thanh toán khi nhận hàng",
  bank: "Chuyển khoản",
  ewallet: "Ví điện tử",
};
