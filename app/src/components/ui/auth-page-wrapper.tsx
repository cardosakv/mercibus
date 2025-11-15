import { AuthLayout } from '@/components/layouts/auth-layout';
import type { ReactNode } from 'react';

interface AuthPageWrapperProps {
  title: string;
  children: ReactNode;
}

export function AuthPageWrapper({ title, children }: AuthPageWrapperProps) {
  return (
    <>
      <title>{title}</title>
      <AuthLayout>{children}</AuthLayout>
    </>
  );
}
