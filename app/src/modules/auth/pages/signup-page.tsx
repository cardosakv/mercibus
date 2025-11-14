import { useState } from 'react';
import { SignupForm } from '../components/signup-form';
import type { SignupData } from '../schemas/signup';
import { authService } from '../api/service';
import type { RegisterRequest } from '../api/types';

export function SignupPage() {
  const [isLoading, setIsLoading] = useState(false);

  const handleSignup = async (data: SignupData) => {
    try {
      setIsLoading(true);

      const payload: RegisterRequest = {
        username: data.username,
        email: data.email,
        password: data.password,
      };

      await authService.register(payload);
    } catch (error) {
      console.error(error);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <SignupForm
      onSubmit={handleSignup}
      isLoading={isLoading}
    />
  );
}
