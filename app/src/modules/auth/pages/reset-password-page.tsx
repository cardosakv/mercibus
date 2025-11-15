import { AuthLayout } from '@/components/layouts/auth-layout';
import { useState } from 'react';
import { authService } from '../api/service';
import { getErrorMessage } from '@/utils/error';
import type { ResetPasswordData } from '../schemas/reset-password';
import { ResetPasswordForm } from '../components/reset-password-form';
import { useSearchParams } from 'react-router-dom';
import { ResetPasswordSuccess } from '../components/reset-password-success';

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
      <title>Mercibus - Reset Password</title>
      <AuthLayout>
        {isResetSuccessful ? (
          <ResetPasswordSuccess />
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
