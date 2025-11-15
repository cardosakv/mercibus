import { useState } from 'react';
import { SignupForm } from '../components/signup-form';
import { type SignupData } from '../schemas/signup';
import { authService } from '../api/service';
import { getErrorMessage } from '@/utils/error';
import { MessageCard } from '@/components/ui/message-card';
import { AuthLayout } from '@/components/layouts/auth-layout';
import { Helmet } from 'react-helmet';

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
    <>
      <Helmet>
        <title>Mercibus - Sign Up</title>
      </Helmet>
      <AuthLayout>
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
      </AuthLayout>
    </>
  );
}
