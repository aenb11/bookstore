
"use client";

import { useState, type FormEvent } from "react";
import { Pencil, Plus, Search, Tags, Trash2 } from "lucide-react";

type Tag = {
  id: number;
  name: string;
};

// Dữ liệu giả để làm giao diện, chưa kết nối Backend
const initialTags: Tag[] = [
  { id: 1, name: "Bán chạy" },
  { id: 2, name: "Sách mới" },
  { id: 3, name: "Nổi bật" },
  { id: 4, name: "Khuyến mãi" },
  { id: 5, name: "Đề xuất" },
];

export default function AdminTagsPage() {
  const [tags, setTags] = useState<Tag[]>(initialTags);
  const [search, setSearch] = useState("");
  const [name, setName] = useState("");
  const [editingId, setEditingId] = useState<number | null>(null);
  const [error, setError] = useState("");

  const filteredTags = tags.filter((tag) =>
    tag.name.toLowerCase().includes(search.trim().toLowerCase())
  );

  function handleSubmit(e: FormEvent<HTMLFormElement>) {
    e.preventDefault();

    const newName = name.trim();

    if (!newName) {
      setError("Vui lòng nhập tên tag.");
      return;
    }

    const duplicated = tags.some(
      (tag) =>
        tag.name.toLowerCase() === newName.toLowerCase() &&
        tag.id !== editingId
    );

    if (duplicated) {
      setError("Tên tag đã tồn tại.");
      return;
    }

    if (editingId !== null) {
      setTags((prev) =>
        prev.map((tag) =>
          tag.id === editingId ? { ...tag, name: newName } : tag
        )
      );
    } else {
      const newId = Math.max(0, ...tags.map((tag) => tag.id)) + 1;
      setTags((prev) => [...prev, { id: newId, name: newName }]);
    }

    resetForm();
  }

  function resetForm() {
    setName("");
    setEditingId(null);
    setError("");
  }

  function handleEdit(tag: Tag) {
    setName(tag.name);
    setEditingId(tag.id);
    setError("");
  }

  function handleDelete(id: number) {
    setTags((prev) => prev.filter((tag) => tag.id !== id));

    if (editingId === id) {
      resetForm();
    }
  }

  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-2xl font-bold tracking-tight">
          Quản lý Tag
        </h1>
        <p className="mt-1 text-sm text-muted-foreground">
          Quản lý các thẻ gắn với sách trong hệ thống Bookstore.
        </p>
      </div>

      <div className="rounded-xl border bg-card p-5 shadow-sm">
        <div className="flex items-center gap-3">
          <div className="rounded-lg bg-primary/10 p-3 text-primary">
            <Tags className="size-6" />
          </div>
          <div>
            <p className="text-sm text-muted-foreground">
              Tổng số Tag
            </p>
            <p className="text-3xl font-bold">{tags.length}</p>
          </div>
        </div>
      </div>

      <div className="rounded-xl border bg-card p-5 shadow-sm">
        <h2 className="mb-4 text-lg font-semibold">
          {editingId !== null ? "Chỉnh sửa Tag" : "Thêm Tag mới"}
        </h2>

        <form onSubmit={handleSubmit} className="space-y-3">
          <label
            htmlFor="tag-name"
            className="block text-sm font-medium"
          >
            Tên Tag
          </label>

          <div className="flex flex-col gap-3 sm:flex-row">
            <input
              id="tag-name"
              type="text"
              value={name}
              onChange={(e) => {
                setName(e.target.value);
                setError("");
              }}
              placeholder="Nhập tên Tag..."
              className="h-10 flex-1 rounded-md border bg-background px-3 text-sm outline-none focus:ring-2 focus:ring-primary"
            />

            <button
              type="submit"
              className="flex h-10 items-center justify-center gap-2 rounded-md bg-primary px-4 text-sm font-medium text-primary-foreground hover:opacity-90"
            >
              <Plus className="size-4" />
              {editingId !== null ? "Lưu thay đổi" : "Thêm Tag"}
            </button>

            {editingId !== null && (
              <button
                type="button"
                onClick={resetForm}
                className="h-10 rounded-md border px-4 text-sm hover:bg-muted"
              >
                Hủy
              </button>
            )}
          </div>

          {error && (
            <p className="text-sm text-red-600">{error}</p>
          )}
        </form>
      </div>

      <div className="overflow-hidden rounded-xl border bg-card shadow-sm">
        <div className="flex flex-col gap-3 border-b p-4 sm:flex-row sm:items-center sm:justify-between">
          <h2 className="text-lg font-semibold">
            Danh sách Tag
          </h2>

          <div className="relative">
            <Search className="absolute left-3 top-1/2 size-4 -translate-y-1/2 text-muted-foreground" />
            <input
              value={search}
              onChange={(e) => setSearch(e.target.value)}
              placeholder="Tìm kiếm Tag..."
              className="h-10 w-full rounded-md border bg-background pl-9 pr-3 text-sm sm:w-64"
            />
          </div>
        </div>

        <div className="overflow-x-auto">
          <table className="w-full text-left text-sm">
            <thead className="border-b bg-muted/50">
              <tr>
                <th className="px-4 py-3">ID</th>
                <th className="px-4 py-3">Tên Tag</th>
                <th className="px-4 py-3 text-right">
                  Thao tác
                </th>
              </tr>
            </thead>

            <tbody>
              {filteredTags.map((tag) => (
                <tr
                  key={tag.id}
                  className="border-b last:border-0 hover:bg-muted/30"
                >
                  <td className="px-4 py-4">{tag.id}</td>
                  <td className="px-4 py-4 font-medium">
                    {tag.name}
                  </td>
                  <td className="px-4 py-4">
                    <div className="flex justify-end gap-2">
                      <button
                        onClick={() => handleEdit(tag)}
                        className="flex items-center gap-1 rounded-md border px-3 py-2 hover:bg-muted"
                      >
                        <Pencil className="size-4" />
                        Sửa
                      </button>

                      <button
                        onClick={() => handleDelete(tag.id)}
                        className="flex items-center gap-1 rounded-md border px-3 py-2 text-red-600 hover:bg-red-50"
                      >
                        <Trash2 className="size-4" />
                        Xóa
                      </button>
                    </div>
                  </td>
                </tr>
              ))}

              {filteredTags.length === 0 && (
                <tr>
                  <td
                    colSpan={3}
                    className="px-4 py-10 text-center text-muted-foreground"
                  >
                    Không tìm thấy Tag phù hợp.
                  </td>
                </tr>
              )}
            </tbody>
          </table>
        </div>
      </div>

      <p className="text-xs text-muted-foreground">
        Dữ liệu minh họa. Các thao tác chưa kết nối Backend.
      </p>
    </div>
  );
}
