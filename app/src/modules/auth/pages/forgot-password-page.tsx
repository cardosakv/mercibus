import { AuthPageWrapper } from '@/components/ui/auth-page-wrapper';
import { useState } from 'react';
import { authService } from '../api/service';
import { getErrorMessage } from '@/utils/error';
import type { ForgotPasswordData } from '../schemas/forgot-password';
import { ForgotPasswordForm } from '../components/forgot-password-form';
import { MessageCard } from '@/components/ui/message-card';

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
    <AuthPageWrapper title="Mercibus - Forgot Password">
      {isAccountFound ? (
        <MessageCard
          title="Check Your Email"
          description="We've sent a password reset email to your inbox. Please check your email to complete the password reset process."
        />
      ) : (
        <ForgotPasswordForm
          onSubmit={handleForgotPassword}
          isLoading={isLoading}
          error={error}
        />
      )}
    </AuthPageWrapper>
  );
}
