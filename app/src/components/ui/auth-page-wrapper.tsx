import { AuthLayout } from '@/components/layouts/auth-layout';
import type { ReactNode } from 'react';
import { Helmet } from 'react-helmet';

interface AuthPageWrapperProps {
  title: string;
  children: ReactNode;
}

export function AuthPageWrapper({ title, children }: AuthPageWrapperProps) {
  return (
    <>
      <Helmet>
        <title>{title}</title>
      </Helmet>
      <AuthLayout>{children}</AuthLayout>
    </>
  );
}
