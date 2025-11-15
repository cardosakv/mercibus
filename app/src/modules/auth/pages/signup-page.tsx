import { useState } from 'react';
import { SignupForm } from '../components/signup-form';
import { type SignupData } from '../schemas/signup';
import { authService } from '../api/service';
import { getErrorMessage } from '@/utils/error';
import { AuthPageWrapper } from '@/components/ui/auth-page-wrapper';
import { MessageCard } from '@/components/ui/message-card';

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
    <AuthPageWrapper title="Mercibus - Sign Up">
      {isSuccess ? (
        <MessageCard
          title="Confirm Your Email"
          description="We've sent a confirmation email to your inbox. Please check your email to complete the registration process."
        />
      ) : (
        <SignupForm
          onSubmit={handleSignup}
          isLoading={isLoading}
          error={error}
        />
      )}
    </AuthPageWrapper>
  );
}
