import { Skeleton } from "@/components/ui/skeleton";

export default function CatalogLoading() {
  return (
    <div className="container-page py-8" aria-busy="true" aria-label="Đang tải">
      <Skeleton className="h-4 w-40" />
      <Skeleton className="mt-6 mb-8 h-10 w-72" />
      <div className="flex gap-10">
        <div className="hidden w-60 shrink-0 space-y-3 lg:block">
          {Array.from({ length: 9 }, (_, i) => (
            <Skeleton key={i} className="h-5 w-full" />
          ))}
        </div>
        <div className="grid flex-1 grid-cols-2 gap-x-4 gap-y-8 sm:grid-cols-3 xl:grid-cols-4">
          {Array.from({ length: 8 }, (_, i) => (
            <div key={i} className="space-y-3">
              <Skeleton className="aspect-[3/4] w-full" />
              <Skeleton className="h-4 w-4/5" />
              <Skeleton className="h-3 w-1/2" />
              <Skeleton className="h-4 w-1/3" />
            </div>
          ))}
        </div>
      </div>
    </div>
  );
}
