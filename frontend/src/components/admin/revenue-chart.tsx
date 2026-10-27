"use client";

import { Area, AreaChart, CartesianGrid, XAxis, YAxis } from "recharts";
import { type ChartConfig, ChartContainer, ChartTooltip, ChartTooltipContent } from "@/components/ui/chart";
import { formatCompact, formatVND } from "@/lib/format";
import { monthlyRevenue } from "@/lib/mock-data";

const chartConfig = {
  revenue: { label: "Doanh thu", color: "var(--chart-1)" },
} satisfies ChartConfig;

export function RevenueChart() {
  return (
    <ChartContainer config={chartConfig} className="aspect-auto h-72 w-full">
      <AreaChart data={monthlyRevenue} margin={{ left: 4, right: 12, top: 8 }}>
        <defs>
          <linearGradient id="fill-revenue" x1="0" y1="0" x2="0" y2="1">
            <stop offset="5%" stopColor="var(--color-revenue)" stopOpacity={0.35} />
            <stop offset="95%" stopColor="var(--color-revenue)" stopOpacity={0.02} />
          </linearGradient>
        </defs>
        <CartesianGrid vertical={false} />
        <XAxis dataKey="month" tickLine={false} axisLine={false} tickMargin={8} />
        <YAxis tickLine={false} axisLine={false} width={48} tickFormatter={(value: number) => formatCompact(value)} />
        <ChartTooltip
          cursor={false}
          content={<ChartTooltipContent formatter={(value) => formatVND(Number(value))} />}
        />
        <Area
          dataKey="revenue"
          type="monotone"
          stroke="var(--color-revenue)"
          strokeWidth={2}
          fill="url(#fill-revenue)"
        />
      </AreaChart>
    </ChartContainer>
  );
}
