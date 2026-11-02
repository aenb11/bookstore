import { NextResponse } from "next/server";
import { clerkMiddleware, createRouteMatcher } from "@clerk/nextjs/server";
import { ADMIN_AREA_ROLES, toRole } from "@/lib/auth/roles";

const isProtectedRoute = createRouteMatcher(["/account(.*)", "/checkout(.*)", "/admin(.*)"]);
const isAdminRoute = createRouteMatcher(["/admin(.*)"]);

/**
 * Clerk session handling plus optimistic route protection. Pages check again on the
 * server through `@/lib/auth/dal`, so this is not the only line of defence.
 */
export default clerkMiddleware(async (auth, request) => {
  if (!isProtectedRoute(request)) return;

  // Not signed in → Clerk redirects to /login and comes back afterwards.
  const { sessionClaims } = await auth.protect();

  // Role comes from the customized session token (see src/types/clerk.d.ts).
  if (isAdminRoute(request) && !ADMIN_AREA_ROLES.includes(toRole(sessionClaims?.metadata?.role))) {
    return NextResponse.redirect(new URL("/", request.url));
  }
});

export const config = {
  matcher: [
    // Skip Next.js internals and static files, unless found in search params.
    "/((?!_next|[^?]*\\.(?:html?|css|js(?!on)|jpe?g|webp|png|gif|svg|ttf|woff2?|ico|csv|docx?|xlsx?|zip|webmanifest)).*)",
    // Always run for API routes.
    "/(api|trpc)(.*)",
  ],
};
