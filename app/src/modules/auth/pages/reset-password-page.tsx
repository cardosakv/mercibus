import { useState } from 'react';
import { authService } from '../api/service';
import { getErrorMessage } from '@/utils/error';
import type { ResetPasswordData } from '../schemas/reset-password';
import { ResetPasswordForm } from '../components/reset-password-form';
import { Link, useSearchParams } from 'react-router-dom';
import { SuccessCard } from '@/components/ui/success-card';
import { ROUTE_PATHS } from '@/routes/paths';
import { Button } from '@/components/ui/button';
import { AuthLayout } from '@/components/layouts/auth-layout';
import { Helmet } from 'react-helmet';

export function ResetPasswordPage() {
  const [searchParams] = useSearchParams();

  const [error, setError] = useState<string | null>(null);
  const [isLoading, setIsLoading] = useState(false);
  const [isResetSuccessful, setIsResetSuccessful] = useState(false);

  const handleResetPassword = async (data: ResetPasswordData) => {
    try {
      setIsLoading(true);
      setError(null);

      await authService.resetPassword({
        userId: searchParams.get('userId') ?? '',
        token: searchParams.get('token') ?? '',
        newPassword: data.password,
      });

      setIsResetSuccessful(true);
    } catch (error) {
      const errorMessage = getErrorMessage(error);
      setError(errorMessage);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <>
      <Helmet>
        <title>Mercibus - Reset Password</title>
      </Helmet>
      <AuthLayout>
        {isResetSuccessful ? (
          <SuccessCard
            title="Reset Successful"
            description="Your password has been successfully reset. You can now log in to your account."
            action={
              <Button asChild>
                <Link to={ROUTE_PATHS.LOGIN}>Go to Login</Link>
              </Button>
            }
          />
        ) : (
          <ResetPasswordForm
            onSubmit={handleResetPassword}
            isLoading={isLoading}
            error={error}
          />
        )}
      </AuthLayout>
    </>
  );
}
