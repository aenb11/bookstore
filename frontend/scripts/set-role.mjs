// Sets a user's role (Clerk publicMetadata.role) by email.
//
//   npm run set-role -- someone@example.com admin
//   npm run set-role -- someone@example.com staff
//   npm run set-role -- someone@example.com customer
//
// Reads CLERK_SECRET_KEY from .env.local. The user must sign out and back in
// for the new role to reach the session token.

const ROLES = ["admin", "staff", "customer"];
const [email, role] = process.argv.slice(2);

if (!email || !ROLES.includes(role)) {
  console.error(`Usage: npm run set-role -- <email> <${ROLES.join("|")}>`);
  process.exit(1);
}

const secret = process.env.CLERK_SECRET_KEY;
if (!secret) {
  console.error("CLERK_SECRET_KEY is missing (expected in frontend/.env.local).");
  process.exit(1);
}

const api = async (path, init = {}) => {
  const res = await fetch(`https://api.clerk.com/v1${path}`, {
    ...init,
    headers: { Authorization: `Bearer ${secret}`, "Content-Type": "application/json", ...init.headers },
  });
  const body = await res.json();
  if (!res.ok) throw new Error(`${res.status} ${JSON.stringify(body)}`);
  return body;
};

const users = await api(`/users?email_address=${encodeURIComponent(email)}`);
if (users.length === 0) {
  console.error(`No Clerk user with email ${email}. Sign up at /register first.`);
  process.exit(1);
}

const user = users[0];
// PATCH /metadata merges, so other public metadata keys are kept.
await api(`/users/${user.id}/metadata`, {
  method: "PATCH",
  body: JSON.stringify({ public_metadata: { role } }),
});
console.log(`✓ ${email} (${user.id}) is now "${role}". Sign out and back in to apply.`);
