import { Star } from "lucide-react";
import { cn } from "@/lib/utils";

type RatingStarsProps = {
  rating: number;
  count?: number;
  size?: "sm" | "md";
  className?: string;
};

export function RatingStars({ rating, count, size = "sm", className }: RatingStarsProps) {
  const iconSize = size === "sm" ? "size-3.5" : "size-4.5";
  return (
    <div className={cn("flex items-center gap-1.5", className)}>
      <div className="flex" role="img" aria-label={`Đánh giá ${rating.toFixed(1)} trên 5 sao`}>
        {Array.from({ length: 5 }, (_, i) => {
          const fill = Math.min(Math.max(rating - i, 0), 1);
          return (
            <span key={i} className={cn("relative", iconSize)}>
              <Star className={cn("absolute inset-0 text-rating/30", iconSize)} fill="currentColor" strokeWidth={0} />
              <span className="absolute inset-0 overflow-hidden" style={{ width: `${fill * 100}%` }}>
                <Star className={cn("text-rating", iconSize)} fill="currentColor" strokeWidth={0} />
              </span>
            </span>
          );
        })}
      </div>
      {count !== undefined && (
        <span className={cn("text-muted-foreground", size === "sm" ? "text-xs" : "text-sm")}>
          ({count.toLocaleString("vi-VN")})
        </span>
      )}
    </div>
  );
}
