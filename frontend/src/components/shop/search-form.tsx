import { Search } from "lucide-react";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { cn } from "@/lib/utils";

/** Plain GET form so search works before (and without) JavaScript. */
export function SearchForm({ className, defaultValue }: { className?: string; defaultValue?: string }) {
  return (
    <form action="/books" role="search" className={cn("flex", className)}>
      <label htmlFor="site-search" className="sr-only">
        Tìm kiếm sách
      </label>
      <Input
        id="site-search"
        name="q"
        type="search"
        defaultValue={defaultValue}
        placeholder="Tìm theo tên sách, tác giả…"
        className="h-10 rounded-r-none border-r-0 bg-card"
      />
      <Button type="submit" className="h-10 rounded-l-none px-5" aria-label="Tìm kiếm">
        <Search />
      </Button>
    </form>
  );
}
