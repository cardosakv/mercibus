import { useState } from 'react';
import { SignupForm } from '../components/signup-form';
import { type SignupData } from '../schemas/signup';
import { authService } from '../api/service';
import { AuthLayout } from '@/components/layouts/auth-layout';
import { getErrorMessage } from '@/utils/error';
import { SignupEmailConfirm } from '../components/signup-email-confirm';

export function SignupPage() {
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [isSuccess, setIsSuccess] = useState(false);

  const handleSignup = async (data: SignupData) => {
    try {
      setIsLoading(true);
      setError(null);

      await authService.register({
        username: data.username,
        email: data.email,
        password: data.password,
      });

      await authService.sendConfirmationEmail({
        email: data.email,
      });

      setIsSuccess(true);
    } catch (error) {
      const errorMessage = getErrorMessage(error);
      setError(errorMessage);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <AuthLayout>
      {isSuccess ? (
        <SignupEmailConfirm />
      ) : (
        <SignupForm
          onSubmit={handleSignup}
          isLoading={isLoading}
          error={error}
        />
      )}
    </AuthLayout>
  );
}
