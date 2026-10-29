import Link from "next/link";
import { cn } from "@/lib/utils";

export function Logo({ href = "/", className }: { href?: string; className?: string }) {
  return (
    <Link href={href} className={cn("text-2xl font-black tracking-tight text-primary", className)}>
      trangsach<span className="text-foreground">.vn</span>
    </Link>
  );
}
