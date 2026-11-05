"use client";

import { useRef, useState } from "react";
import type { FormEvent } from "react";
import Link from "next/link";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import {
    Card,
    CardContent,
    CardDescription,
    CardFooter,
    CardHeader,
    CardTitle,
} from "@/components/ui/card";

export default function ForgotPasswordPage() {
    const [email, setEmail] = useState("");
    const [error, setError] = useState("");
    const [message, setMessage] = useState("");
    const [isLoading, setIsLoading] = useState(false);
    const submitting = useRef(false);

    async function handleSubmit(event: FormEvent<HTMLFormElement>) {
        event.preventDefault();

        if (submitting.current) return;

        setError("");
        setMessage("");

        const value = email.trim();

        if (!value) {
            setError("Vui lòng nhập email.");
            return;
        }

        if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value)) {
            setError("Email không đúng định dạng.");
            return;
        }

        submitting.current = true;
        setIsLoading(true);

        try {
            // Mô phỏng xử lí để kiểm tra giao diện.
            // Thay bằng lời gọi API khi backend đã sẵn sàng.
            await new Promise<void>((resolve) => setTimeout(resolve, 1000));

            setMessage(
                "Mô phỏng thành công. Khi kết nối API, nếu email này đã được " +
                "đăng ký, bạn sẽ nhận được hướng dẫn đặt lại mật khẩu. " +
                "Hiện chưa có email nào được gửi."
            );
        } catch {
            setError("Không thể gửi yêu cầu. Vui lòng thử lại.");
        } finally {
            submitting.current = false;
            setIsLoading(false);
        }
    }

    return (
        <div className="container-page flex justify-center py-16">
            <Card className="w-full max-w-md">
                <CardHeader className="text-center">
                    <CardTitle className="font-heading text-3xl">
                        Quên mật khẩu?
                    </CardTitle>
                    <CardDescription>
                        Nhập email đã đăng ký để yêu cầu hướng dẫn đặt lại mật khẩu.
                    </CardDescription>
                </CardHeader>

                <CardContent>
                    <form
                        onSubmit={handleSubmit}
                        noValidate
                        aria-busy={isLoading}
                        className="space-y-5"
                    >
                        <div className="space-y-2">
                            <Label htmlFor="email">Email</Label>
                            <Input
                                id="email"
                                name="email"
                                type="email"
                                autoComplete="email"
                                placeholder="email@cuaban.com"
                                value={email}
                                required
                                disabled={isLoading}
                                aria-invalid={Boolean(error)}
                                aria-describedby={error ? "email-error" : undefined}
                                onChange={(event) => {
                                    setEmail(event.target.value);
                                    setError("");
                                    setMessage("");
                                }}
                                className="h-10"
                            />
                            {error && (
                                <p id="email-error" role="alert" className="text-sm text-destructive">
                                    {error}
                                </p>
                            )}
                        </div>

                        {message && (
                            <p role="status" className="rounded-md border bg-muted p-3 text-sm">
                                {message}
                            </p>
                        )}

                        <Button type="submit" disabled={isLoading} className="h-11 w-full">
                            {isLoading ? "Đang gửi..." : "Gửi yêu cầu"}
                        </Button>
                        <p className="text-center text-xs text-muted-foreground">
                            Bản thử nghiệm giao diện, chưa gửi email thực tế.
                        </p>
                    </form>
                </CardContent>

                <CardFooter className="justify-center">
                    <Link href="/login" className="text-sm text-primary hover:underline">
                        Quay lại đăng nhập
                    </Link>
                </CardFooter>
            </Card>
        </div>
    );
}
