import Link from "next/link";
import { Button } from "@/components/ui/button";

export default function BookNotFound() {
  return (
    <div className="container-page flex flex-col items-center gap-4 py-24 text-center">
      <p className="font-heading text-6xl font-semibold text-primary">404</p>
      <h1 className="text-2xl font-semibold">Không tìm thấy cuốn sách này</h1>
      <p className="max-w-md text-muted-foreground">Có thể sách đã ngừng kinh doanh hoặc đường dẫn không chính xác.</p>
      <Button asChild>
        <Link href="/books">Quay lại cửa hàng</Link>
      </Button>
    </div>
  );
}
