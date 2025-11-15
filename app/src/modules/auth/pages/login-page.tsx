import { AuthPageWrapper } from '@/components/ui/auth-page-wrapper';
import { LoginForm } from '../components/login-form';
import { useState } from 'react';
import type { LoginData } from '../schemas/login';
import { authService } from '../api/service';
import { getErrorMessage } from '@/utils/error';
import { setTokens } from '@/utils/token';
import { useNavigate } from 'react-router-dom';
import { ROUTE_PATHS } from '@/routes/paths';

export function LoginPage() {
  const navigate = useNavigate();

  const [error, setError] = useState<string | null>(null);
  const [isLoading, setIsLoading] = useState(false);

  const handleLogin = async (data: LoginData) => {
    try {
      setIsLoading(true);
      setError(null);

      const response = await authService.login({
        username: data.username,
        password: data.password,
      });

      setTokens(response.accessToken, response.refreshToken);
      navigate(ROUTE_PATHS.HOME);
    } catch (error) {
      const errorMessage = getErrorMessage(error);
      setError(errorMessage);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <AuthPageWrapper title="Mercibus - Login">
      <LoginForm
        onSubmit={handleLogin}
        isLoading={isLoading}
        error={error}
      />
    </AuthPageWrapper>
  );
}
