import { AuthLayout } from '@/components/layouts/auth-layout';
import { useState } from 'react';
import { authService } from '../api/service';
import { getErrorMessage } from '@/utils/error';
import type { ForgotPasswordData } from '../schemas/forgot-password';
import { ForgotPasswordForm } from '../components/forgot-password-form';
import { ForgotPasswordEmailSent } from '../components/forgot-password-email-sent';

export function ForgotPasswordPage() {
  const [error, setError] = useState<string | null>(null);
  const [isLoading, setIsLoading] = useState(false);
  const [isAccountFound, setIsAccountFound] = useState(false);

  const handleForgotPassword = async (data: ForgotPasswordData) => {
    try {
      setIsLoading(true);
      setError(null);

      await authService.forgotPassword({
        username: data.username,
      });

      setIsAccountFound(true);
    } catch (error) {
      const errorMessage = getErrorMessage(error);
      setError(errorMessage);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <AuthLayout>
      {isAccountFound ? (
        <ForgotPasswordEmailSent />
      ) : (
        <ForgotPasswordForm
          onSubmit={handleForgotPassword}
          isLoading={isLoading}
          error={error}
        />
      )}
    </AuthLayout>
  );
}
