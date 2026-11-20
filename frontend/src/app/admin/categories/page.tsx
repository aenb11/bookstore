
"use client";

import { useState, type FormEvent } from "react";
import { FolderOpen, Pencil, Plus, Search, Trash2 } from "lucide-react";

type Category = {
  id: number;
  name: string;
  description: string;
};

// Mock data - chưa kết nối Backend
const initialCategories: Category[] = [
  { id: 1, name: "Văn học", description: "Tiểu thuyết, truyện ngắn, thơ" },
  { id: 2, name: "Kinh tế", description: "Kinh doanh, tài chính, quản trị" },
  { id: 3, name: "Kỹ năng sống", description: "Phát triển bản thân và kỹ năng" },
  { id: 4, name: "Thiếu nhi", description: "Sách dành cho trẻ em" },
  { id: 5, name: "Công nghệ", description: "Lập trình và công nghệ thông tin" },
];

export default function AdminCategoriesPage() {
  const [categories, setCategories] = useState(initialCategories);
  const [search, setSearch] = useState("");
  const [name, setName] = useState("");
  const [description, setDescription] = useState("");
  const [editingId, setEditingId] = useState<number | null>(null);
  const [error, setError] = useState("");

  const filteredCategories = categories.filter((category) =>
    category.name.toLowerCase().includes(search.trim().toLowerCase())
  );

  function resetForm() {
    setName("");
    setDescription("");
    setEditingId(null);
    setError("");
  }

  function handleSubmit(e: FormEvent<HTMLFormElement>) {
    e.preventDefault();

    const newName = name.trim();

    if (!newName) {
      setError("Vui lòng nhập tên danh mục.");
      return;
    }

    const duplicated = categories.some(
      (category) =>
        category.name.toLowerCase() === newName.toLowerCase() &&
        category.id !== editingId
    );

    if (duplicated) {
      setError("Tên danh mục đã tồn tại.");
      return;
    }

    if (editingId !== null) {
      setCategories((prev) =>
        prev.map((category) =>
          category.id === editingId
            ? { ...category, name: newName, description: description.trim() }
            : category
        )
      );
    } else {
      const newId = Math.max(0, ...categories.map((c) => c.id)) + 1;
      setCategories((prev) => [
        ...prev,
        { id: newId, name: newName, description: description.trim() },
      ]);
    }

    resetForm();
  }

  function handleEdit(category: Category) {
    setEditingId(category.id);
    setName(category.name);
    setDescription(category.description);
    setError("");
  }

  function handleDelete(id: number) {
    if (!window.confirm("Bạn có chắc muốn xóa danh mục này?")) {
      return;
    }

    setCategories((prev) => prev.filter((category) => category.id !== id));

    if (editingId === id) {
      resetForm();
    }
  }

  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-2xl font-bold tracking-tight">
          Quản lý danh mục sách
        </h1>
        <p className="mt-1 text-sm text-muted-foreground">
          Quản lý các danh mục sách trong hệ thống Bookstore.
        </p>
      </div>

      <div className="rounded-xl border bg-card p-5 shadow-sm">
        <div className="flex items-center gap-3">
          <div className="rounded-lg bg-primary/10 p-3 text-primary">
            <FolderOpen className="size-6" />
          </div>
          <div>
            <p className="text-sm text-muted-foreground">
              Tổng số danh mục
            </p>
            <p className="text-3xl font-bold">{categories.length}</p>
          </div>
        </div>
      </div>

      <div className="rounded-xl border bg-card p-5 shadow-sm">
        <h2 className="mb-4 text-lg font-semibold">
          {editingId !== null
            ? "Chỉnh sửa danh mục"
            : "Thêm danh mục mới"}
        </h2>

        <form onSubmit={handleSubmit} className="space-y-4">
          <div>
            <label htmlFor="category-name" className="mb-2 block text-sm font-medium">
              Tên danh mục
            </label>
            <input
              id="category-name"
              value={name}
              onChange={(e) => {
                setName(e.target.value);
                setError("");
              }}
              placeholder="Nhập tên danh mục..."
              className="h-10 w-full rounded-md border bg-background px-3 text-sm outline-none focus:ring-2 focus:ring-primary"
            />
          </div>

          <div>
            <label htmlFor="category-description" className="mb-2 block text-sm font-medium">
              Mô tả
            </label>
            <textarea
              id="category-description"
              value={description}
              onChange={(e) => setDescription(e.target.value)}
              placeholder="Nhập mô tả danh mục..."
              rows={3}
              className="w-full rounded-md border bg-background p-3 text-sm outline-none focus:ring-2 focus:ring-primary"
            />
          </div>

          {error && <p className="text-sm text-red-600">{error}</p>}

          <div className="flex gap-3">
            <button
              type="submit"
              className="flex items-center gap-2 rounded-md bg-primary px-4 py-2 text-sm font-medium text-primary-foreground hover:opacity-90"
            >
              <Plus className="size-4" />
              {editingId !== null ? "Lưu thay đổi" : "Thêm danh mục"}
            </button>

            {editingId !== null && (
              <button
                type="button"
                onClick={resetForm}
                className="rounded-md border px-4 py-2 text-sm hover:bg-muted"
              >
                Hủy
              </button>
            )}
          </div>
        </form>
      </div>

      <div className="overflow-hidden rounded-xl border bg-card shadow-sm">
        <div className="flex flex-col gap-3 border-b p-4 sm:flex-row sm:items-center sm:justify-between">
          <h2 className="text-lg font-semibold">
            Danh sách danh mục
          </h2>

          <div className="relative">
            <Search className="absolute left-3 top-1/2 size-4 -translate-y-1/2 text-muted-foreground" />
            <input
              value={search}
              onChange={(e) => setSearch(e.target.value)}
              placeholder="Tìm kiếm danh mục..."
              className="h-10 w-full rounded-md border bg-background pl-9 pr-3 text-sm sm:w-64"
            />
          </div>
        </div>

        <div className="overflow-x-auto">
          <table className="w-full text-left text-sm">
            <thead className="border-b bg-muted/50">
              <tr>
                <th className="px-4 py-3">ID</th>
                <th className="px-4 py-3">Tên danh mục</th>
                <th className="px-4 py-3">Mô tả</th>
                <th className="px-4 py-3 text-right">Thao tác</th>
              </tr>
            </thead>

            <tbody>
              {filteredCategories.map((category) => (
                <tr key={category.id} className="border-b last:border-0 hover:bg-muted/30">
                  <td className="px-4 py-4">{category.id}</td>
                  <td className="px-4 py-4 font-medium">{category.name}</td>
                  <td className="px-4 py-4">{category.description}</td>
                  <td className="px-4 py-4">
                    <div className="flex justify-end gap-2">
                      <button
                        type="button"
                        onClick={() => handleEdit(category)}
                        className="flex items-center gap-1 rounded-md border px-3 py-2 hover:bg-muted"
                      >
                        <Pencil className="size-4" />
                        Sửa
                      </button>
                      <button
                        type="button"
                        onClick={() => handleDelete(category.id)}
                        className="flex items-center gap-1 rounded-md border px-3 py-2 text-red-600 hover:bg-red-50"
                      >
                        <Trash2 className="size-4" />
                        Xóa
                      </button>
                    </div>
                  </td>
                </tr>
              ))}

              {filteredCategories.length === 0 && (
                <tr>
                  <td colSpan={4} className="px-4 py-10 text-center text-muted-foreground">
                    Không tìm thấy danh mục phù hợp.
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
