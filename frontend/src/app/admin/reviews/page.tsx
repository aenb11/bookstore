
"use client";

import { useMemo, useState } from "react";
import {
  Eye,
  EyeOff,
  MessageSquare,
  Search,
  Star,
} from "lucide-react";

type Review = {
  id: number;
  userName: string;
  bookTitle: string;
  rating: number;
  comment: string;
  createdAt: string;
  status: "visible" | "hidden";
};

// Mock data - chưa kết nối Backend
const initialReviews: Review[] = [
  {
    id: 1,
    userName: "Nguyễn Minh Anh",
    bookTitle: "Nhà giả kim",
    rating: 5,
    comment: "Sách rất hay và ý nghĩa, mình rất thích.",
    createdAt: "15/11/2026",
    status: "visible",
  },
  {
    id: 2,
    userName: "Trần Hoàng Phúc",
    bookTitle: "Đắc nhân tâm",
    rating: 4,
    comment: "Nội dung bổ ích, dễ đọc và áp dụng.",
    createdAt: "16/11/2026",
    status: "visible",
  },
  {
    id: 3,
    userName: "Lê Thanh Hà",
    bookTitle: "Dế mèn phiêu lưu ký",
    rating: 3,
    comment: "Nội dung khá thú vị.",
    createdAt: "17/11/2026",
    status: "visible",
  },
  {
    id: 4,
    userName: "Phạm Gia Hân",
    bookTitle: "Atomic Habits",
    rating: 2,
    comment: "Sách không phù hợp với kỳ vọng của mình.",
    createdAt: "18/11/2026",
    status: "hidden",
  },
  {
    id: 5,
    userName: "Võ Quốc Bảo",
    bookTitle: "Tuổi trẻ đáng giá bao nhiêu",
    rating: 5,
    comment: "Một cuốn sách rất đáng đọc.",
    createdAt: "19/11/2026",
    status: "visible",
  },
];

export default function AdminReviewsPage() {
  const [reviews, setReviews] = useState(initialReviews);
  const [search, setSearch] = useState("");
  const [ratingFilter, setRatingFilter] = useState("all");
  const [statusFilter, setStatusFilter] = useState("all");

  const filteredReviews = useMemo(() => {
    return reviews.filter((review) => {
      const keyword = search.trim().toLowerCase();

      const matchesSearch =
        review.userName.toLowerCase().includes(keyword) ||
        review.bookTitle.toLowerCase().includes(keyword) ||
        review.comment.toLowerCase().includes(keyword);

      const matchesRating =
        ratingFilter === "all" ||
        review.rating === Number(ratingFilter);

      const matchesStatus =
        statusFilter === "all" ||
        review.status === statusFilter;

      return matchesSearch && matchesRating && matchesStatus;
    });
  }, [reviews, search, ratingFilter, statusFilter]);

  function toggleStatus(id: number) {
    setReviews((prev) =>
      prev.map((review) =>
        review.id === id
          ? {
              ...review,
              status:
                review.status === "visible"
                  ? ("hidden" as const)
                  : ("visible" as const),
            }
          : review
      )
    );
  }

  const visibleCount = reviews.filter(
    (review) => review.status === "visible"
  ).length;

  const hiddenCount = reviews.length - visibleCount;

  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-2xl font-bold tracking-tight">
          Quản lý đánh giá sách
        </h1>
        <p className="mt-1 text-sm text-muted-foreground">
          Xem và quản lý đánh giá của khách hàng.
        </p>
      </div>

      <div className="grid gap-4 sm:grid-cols-3">
        <StatCard
          title="Tổng đánh giá"
          value={reviews.length}
        />
        <StatCard
          title="Đang hiển thị"
          value={visibleCount}
        />
        <StatCard
          title="Đã ẩn"
          value={hiddenCount}
        />
      </div>

      <div className="overflow-hidden rounded-xl border bg-card shadow-sm">
        <div className="border-b p-4">
          <h2 className="mb-4 flex items-center gap-2 text-lg font-semibold">
            <MessageSquare className="size-5" />
            Danh sách đánh giá
          </h2>

          <div className="flex flex-col gap-3 md:flex-row">
            <div className="relative flex-1">
              <Search className="absolute left-3 top-1/2 size-4 -translate-y-1/2 text-muted-foreground" />
              <input
                value={search}
                onChange={(e) => setSearch(e.target.value)}
                placeholder="Tìm theo khách hàng, sách, nội dung..."
                aria-label="Tìm kiếm đánh giá"
                className="h-10 w-full rounded-md border bg-background pl-9 pr-3 text-sm outline-none focus:ring-2 focus:ring-primary"
              />
            </div>

            <select
              value={ratingFilter}
              onChange={(e) => setRatingFilter(e.target.value)}
              aria-label="Lọc theo số sao"
              className="h-10 rounded-md border bg-background px-3 text-sm"
            >
              <option value="all">Tất cả số sao</option>
              {[5, 4, 3, 2, 1].map((rating) => (
                <option key={rating} value={rating}>
                  {rating} sao
                </option>
              ))}
            </select>

            <select
              value={statusFilter}
              onChange={(e) => setStatusFilter(e.target.value)}
              aria-label="Lọc theo trạng thái"
              className="h-10 rounded-md border bg-background px-3 text-sm"
            >
              <option value="all">Tất cả trạng thái</option>
              <option value="visible">Đang hiển thị</option>
              <option value="hidden">Đã ẩn</option>
            </select>
          </div>
        </div>

        <div className="overflow-x-auto">
          <table className="w-full min-w-[850px] text-left text-sm">
            <thead className="border-b bg-muted/50">
              <tr>
                <th className="px-4 py-3">ID</th>
                <th className="px-4 py-3">Khách hàng</th>
                <th className="px-4 py-3">Sách</th>
                <th className="px-4 py-3">Đánh giá</th>
                <th className="px-4 py-3">Nội dung</th>
                <th className="px-4 py-3">Ngày đăng</th>
                <th className="px-4 py-3">Trạng thái</th>
                <th className="px-4 py-3 text-right">
                  Thao tác
                </th>
              </tr>
            </thead>

            <tbody>
              {filteredReviews.map((review) => (
                <tr
                  key={review.id}
                  className="border-b last:border-0 hover:bg-muted/30"
                >
                  <td className="px-4 py-4">
                    {review.id}
                  </td>
                  <td className="px-4 py-4 font-medium">
                    {review.userName}
                  </td>
                  <td className="px-4 py-4">
                    {review.bookTitle}
                  </td>
                  <td className="px-4 py-4">
                    <span className="flex items-center gap-1">
                      <Star className="size-4 fill-yellow-500 text-yellow-500" />
                      {review.rating}/5
                    </span>
                  </td>
                  <td className="max-w-xs px-4 py-4">
                    <p className="whitespace-normal break-words">
                      {review.comment}
                    </p>
                  </td>
                  <td className="px-4 py-4">
                    {review.createdAt}
                  </td>
                  <td className="px-4 py-4">
                    <span
                      className={`rounded-full px-2.5 py-1 text-xs font-medium ${
                        review.status === "visible"
                          ? "bg-green-100 text-green-700"
                          : "bg-red-100 text-red-700"
                      }`}
                    >
                      {review.status === "visible"
                        ? "Đang hiển thị"
                        : "Đã ẩn"}
                    </span>
                  </td>
                  <td className="px-4 py-4">
                    <button
                      type="button"
                      onClick={() => toggleStatus(review.id)}
                      className="flex items-center gap-1 rounded-md border px-3 py-2 hover:bg-muted"
                    >
                      {review.status === "visible" ? (
                        <>
                          <EyeOff className="size-4" />
                          Ẩn
                        </>
                      ) : (
                        <>
                          <Eye className="size-4" />
                          Hiện
                        </>
                      )}
                    </button>
                  </td>
                </tr>
              ))}

              {filteredReviews.length === 0 && (
                <tr>
                  <td
                    colSpan={8}
                    className="px-4 py-10 text-center text-muted-foreground"
                  >
                    Không tìm thấy đánh giá phù hợp.
                  </td>
                </tr>
              )}
            </tbody>
          </table>
        </div>
      </div>

      <p className="text-xs text-muted-foreground">
        Dữ liệu minh họa. Thao tác ẩn/hiện chỉ thay đổi
        tạm thời trên giao diện, chưa kết nối Backend.
      </p>
    </div>
  );
}

function StatCard({
  title,
  value,
}: {
  title: string;
  value: number;
}) {
  return (
    <div className="rounded-xl border bg-card p-5 shadow-sm">
      <p className="text-sm text-muted-foreground">
        {title}
      </p>
      <p className="mt-2 text-3xl font-bold">
        {value}
      </p>
    </div>
  );
}
