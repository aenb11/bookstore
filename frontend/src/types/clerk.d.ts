import type { Role } from "@/lib/auth/roles";

declare global {
  /** Set in Clerk Dashboard → Users → (user) → Metadata → Public: `{ "role": "admin" }`. */
  interface UserPublicMetadata {
    role?: Role;
  }

  /**
   * Clerk Dashboard → Sessions → Customize session token:
   * `{ "metadata": "{{user.public_metadata}}" }`
   * so the proxy can check roles without an API call.
   */
  interface CustomJwtSessionClaims {
    metadata?: { role?: Role };
  }
}

export {};
