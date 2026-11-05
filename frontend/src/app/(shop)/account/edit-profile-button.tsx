"use client";

import { useClerk } from "@clerk/nextjs";
import { Button } from "@/components/ui/button";

/** Opens Clerk's profile modal (name, avatar, email, password, connected Google account). */
export function EditProfileButton() {
  const { openUserProfile } = useClerk();
  return (
    <Button variant="outline" size="sm" onClick={() => openUserProfile()}>
      Chỉnh sửa
    </Button>
  );
}
