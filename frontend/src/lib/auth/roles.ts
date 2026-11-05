/**
 * App roles, stored in Clerk as `publicMetadata.role`. Users without a role are customers.
 */
export const ROLES = ["admin", "staff", "customer"] as const;
export type Role = (typeof ROLES)[number];

/** Roles allowed into /admin. */
export const ADMIN_AREA_ROLES: readonly Role[] = ["admin", "staff"];

export function toRole(value: unknown): Role {
  return typeof value === "string" && (ROLES as readonly string[]).includes(value) ? (value as Role) : "customer";
}

export function hasAnyRole(user: { role: Role } | null | undefined, allowed: readonly Role[]) {
  return !!user && allowed.includes(user.role);
}
