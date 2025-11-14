import { useState } from 'react';
import { SignupForm } from '../components/signup-form';
import type { SignupData } from '../schemas/signup';
import { authService } from '../api/service';
import type { RegisterRequest } from '../api/types';
import logo from '@/assets/mercibus.png';
import { getErrorMessage } from '@/utils/error';

export function SignupPage() {
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const handleSignup = async (data: SignupData) => {
    try {
      setIsLoading(true);
      setError(null);

      const payload: RegisterRequest = {
        username: data.username,
        email: data.email,
        password: data.password,
      };

      await authService.register(payload);
    } catch (error) {
      const errorMessage = getErrorMessage(error);
      setError(errorMessage);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="bg-muted flex min-h-svh flex-col items-center justify-start p-12">
      <div className="flex w-full max-w-md flex-col gap-6">
        <a
          href="/"
          className="flex items-center gap-2 self-center font-bold text-primary"
        >
          <div className="flex size-6 items-center justify-center rounded-md">
            <img
              src={logo}
              alt="Mercibus Logo"
            />
          </div>
          Mercibus
        </a>
        <SignupForm
          onSubmit={handleSignup}
          isLoading={isLoading}
          error={error}
        />
      </div>
    </div>
  );
}
