import { useState } from 'react';
import { SignupForm } from '../components/signup-form';
import { type SignupData } from '../schemas/signup';
import { authService } from '../api/service';
import { ROUTE_PATHS } from '@/routes/paths';
import { useNavigate } from 'react-router-dom';
import { AuthLayout } from '@/components/layouts/auth-layout';
import { getErrorMessage } from '@/utils/error';

export function SignupPage() {
  const navigate = useNavigate();
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

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

      navigate(ROUTE_PATHS.SIGNUP_SUCCESS);
    } catch (error) {
      const errorMessage = getErrorMessage(error);
      setError(errorMessage);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <AuthLayout>
      <SignupForm
        onSubmit={handleSignup}
        isLoading={isLoading}
        error={error}
      />
    </AuthLayout>
  );
}
