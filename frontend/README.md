This is a [Next.js](https://nextjs.org) project bootstrapped with [`create-next-app`](https://nextjs.org/docs/app/api-reference/cli/create-next-app).

## Getting Started

First, run the development server:

```bash
npm run dev
# or
yarn dev
# or
pnpm dev
# or
bun dev
```

Open [http://localhost:3000](http://localhost:3000) with your browser to see the result.

You can start editing the page by modifying `app/page.tsx`. The page auto-updates as you edit the file.

This project uses [`next/font`](https://nextjs.org/docs/app/building-your-application/optimizing/fonts) to automatically optimize and load [Geist](https://vercel.com/font), a new font family for Vercel.

## Authentication (Clerk)

Sign-in and sign-up use [Clerk](https://clerk.com) (free tier: 10,000 monthly users).
`/login` and `/register` render Clerk's `<SignIn />` / `<SignUp />` components, in
Vietnamese and with the shadcn theme.

### 1. Set up the Clerk application

In the [Clerk Dashboard](https://dashboard.clerk.com):

1. **Create application** and enable *Email* and *Google* as sign-in options.
   (In development Clerk uses shared Google credentials, so Google works right away;
   for production, add your own Google OAuth client.)
2. **Configure → API keys**: copy the publishable and secret keys.
3. **Configure → Sessions → Customize session token**, add:
   ```json
   { "metadata": "{{user.public_metadata}}" }
   ```
   This puts the user's role in the session token, so `src/proxy.ts` can check it.
4. **Roles**: after the user has signed up, run
   ```bash
   npm run set-role -- someone@example.com admin   # or staff / customer
   ```
   (or set `{ "role": "admin" }` in the user's public metadata in the Dashboard).
   Users without a role are customers. Sign out and back in after changing a role.

### 2. Set environment variables

```bash
cp .env.example .env.local
# fill in NEXT_PUBLIC_CLERK_PUBLISHABLE_KEY and CLERK_SECRET_KEY
```

### 3. Development admin account

In development, use a Clerk test email (any address containing `+clerk_test`).
Clerk sends no real email for these, and the verification code is always `424242`.

1. Go to `/register` and sign up with `admin+clerk_test@example.com` and a password of your choice.
2. On the "Verify email" screen, enter `424242`.
3. Make the account an admin:
   ```bash
   npm run set-role -- admin+clerk_test@example.com admin
   ```
4. Sign out and back in so the session picks up the role.

Forgot the password? Set a new one in the Clerk Dashboard under **Users**.
Test emails only work with development keys (`pk_test_` / `sk_test_`).

### How it works

| Path | Access |
| --- | --- |
| `/login`, `/register` | Clerk sign-in / sign-up (forgot password, email verification and Google are built in) |
| `/account`, `/checkout` | Signed-in users |
| `/admin` | `publicMetadata.role` is `admin` or `staff` |

On the server, read the user with `getCurrentUser()`, `requireUser()` and `requireRole()`
from `src/lib/auth/dal.ts`. Use `getAccessToken()` to call the backend API as the user
(the backend should verify it as a Clerk JWT). The user's Clerk id matches
`Account.ClerkId` in the backend.

## Learn More

To learn more about Next.js, take a look at the following resources:

- [Next.js Documentation](https://nextjs.org/docs) - learn about Next.js features and API.
- [Learn Next.js](https://nextjs.org/learn) - an interactive Next.js tutorial.

You can check out [the Next.js GitHub repository](https://github.com/vercel/next.js) - your feedback and contributions are welcome!

## Deploy on Vercel

The easiest way to deploy your Next.js app is to use the [Vercel Platform](https://vercel.com/new?utm_medium=default-template&filter=next.js&utm_source=create-next-app&utm_campaign=create-next-app-readme) from the creators of Next.js.

Check out our [Next.js deployment documentation](https://nextjs.org/docs/app/building-your-application/deploying) for more details.
