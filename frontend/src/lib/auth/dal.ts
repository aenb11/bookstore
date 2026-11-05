import "server-only";
import { cache } from "react";
import { redirect } from "next/navigation";
import { auth, currentUser } from "@clerk/nextjs/server";
import { hasAnyRole, toRole, type Role } from "./roles";

/**
 * Data Access Layer for auth. Server Components, Server Actions and Route Handlers
 * read the user only through these helpers. They read the request, so callers must
 * render behind a <Suspense> boundary (Cache Components).
 */
export type AppUser = {
  /** Clerk user id; matches Account.ClerkId in the backend. */
  id: string;
  name: string;
  /** What to greet the user by. */
  givenName: string;
  email: string;
  phone: string | null;
  imageUrl: string;
  role: Role;
};

/** The logged-in user, or null. Deduplicated per request. */
export const getCurrentUser = cache(async (): Promise<AppUser | null> => {
  const user = await currentUser();
  if (!user) return null;

  const email = user.primaryEmailAddress?.emailAddress ?? "";
  const name = user.fullName ?? user.username ?? email;
  return {
    id: user.id,
    name,
    givenName: user.firstName ?? name,
    email,
    phone: user.primaryPhoneNumber?.phoneNumber ?? null,
    imageUrl: user.imageUrl,
    // Read from the user record (not the session token) so role changes apply immediately.
    role: toRole(user.publicMetadata.role),
  };
});

/** Session token for calling the backend API on behalf of the user. */
export async function getAccessToken(): Promise<string | null> {
  return (await auth()).getToken();
}

export async function requireUser(returnTo = "/"): Promise<AppUser> {
  const user = await getCurrentUser();
  if (!user) redirect(`/login?redirect_url=${encodeURIComponent(returnTo)}`);
  return user;
}

export async function requireRole(allowed: readonly Role[], returnTo = "/"): Promise<AppUser> {
  const user = await requireUser(returnTo);
  if (!hasAnyRole(user, allowed)) redirect("/");
  return user;
}
