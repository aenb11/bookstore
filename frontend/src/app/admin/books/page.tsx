import type { Metadata } from "next";
import { PageHeader } from "@/components/admin/page-header";
import { BooksTable } from "./books-table";
import { BookFormDialog } from "./book-form-dialog";

export const metadata: Metadata = { title: "Sách" };

export default function AdminBooksPage() {
  return (
    <>
      <PageHeader
        title="Quản lý sách"
        description="Thêm, chỉnh sửa và theo dõi tồn kho các đầu sách."
        actions={<BookFormDialog />}
      />
      <BooksTable />
    </>
  );
}
