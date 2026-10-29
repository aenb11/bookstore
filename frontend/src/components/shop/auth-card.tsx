import Link from "next/link";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardDescription, CardFooter, CardHeader, CardTitle } from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Separator } from "@/components/ui/separator";

type AuthField = {
  id: string;
  label: string;
  type?: string;
  autoComplete: string;
  placeholder?: string;
};

type AuthCardProps = {
  title: string;
  description: string;
  fields: AuthField[];
  submitLabel: string;
  extra?: React.ReactNode;
  footer: React.ReactNode;
};

/** UI-only auth form shared by login and register; no submission is wired up yet. */
export function AuthCard({ title, description, fields, submitLabel, extra, footer }: AuthCardProps) {
  return (
    <div className="container-page flex justify-center py-16">
      <Card className="w-full max-w-md">
        <CardHeader className="text-center">
          <CardTitle className="font-heading text-3xl">{title}</CardTitle>
          <CardDescription>{description}</CardDescription>
        </CardHeader>
        <CardContent className="space-y-5">
          <Button variant="outline" size="lg" className="h-11 w-full" type="button">
            <GoogleIcon /> Tiếp tục với Google
          </Button>
          <div className="flex items-center gap-3 text-xs text-muted-foreground">
            <Separator className="flex-1" /> hoặc <Separator className="flex-1" />
          </div>
          <form className="space-y-4">
            {fields.map((field) => (
              <div key={field.id} className="space-y-2">
                <Label htmlFor={field.id}>{field.label}</Label>
                <Input
                  id={field.id}
                  name={field.id}
                  type={field.type ?? "text"}
                  autoComplete={field.autoComplete}
                  placeholder={field.placeholder}
                  required
                  className="h-10"
                />
              </div>
            ))}
            {extra}
            <Button type="submit" size="lg" className="h-11 w-full">
              {submitLabel}
            </Button>
          </form>
        </CardContent>
        <CardFooter className="justify-center text-sm text-muted-foreground">{footer}</CardFooter>
      </Card>
    </div>
  );
}

export function AuthLink({ href, children }: { href: string; children: React.ReactNode }) {
  return (
    <Link href={href} className="ml-1 font-medium text-primary hover:underline">
      {children}
    </Link>
  );
}

function GoogleIcon() {
  return (
    <svg viewBox="0 0 24 24" aria-hidden className="size-4">
      <path fill="#4285F4" d="M22.56 12.25c0-.78-.07-1.53-.2-2.25H12v4.26h5.92a5.06 5.06 0 0 1-2.2 3.32v2.77h3.57c2.08-1.92 3.27-4.74 3.27-8.1z" />
      <path fill="#34A853" d="M12 23c2.97 0 5.46-.98 7.28-2.66l-3.57-2.77c-.98.66-2.23 1.06-3.71 1.06-2.86 0-5.29-1.93-6.16-4.53H2.18v2.84A11 11 0 0 0 12 23z" />
      <path fill="#FBBC05" d="M5.84 14.1a6.6 6.6 0 0 1 0-4.2V7.06H2.18a11 11 0 0 0 0 9.88l3.66-2.84z" />
      <path fill="#EA4335" d="M12 5.38c1.62 0 3.06.56 4.21 1.64l3.15-3.15A10.96 10.96 0 0 0 12 1 11 11 0 0 0 2.18 7.06l3.66 2.84C6.71 7.31 9.14 5.38 12 5.38z" />
    </svg>
  );
}
