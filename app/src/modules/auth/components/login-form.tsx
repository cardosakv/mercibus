import { Button } from '@/components/ui/button';
import { FormContainer } from '@/components/ui/form-container';
import { FormField } from '@/components/ui/form-field';
import { Field, FieldDescription, FieldGroup } from '@/components/ui/field';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { loginSchema, type LoginData } from '../schemas/login';
import { Link } from 'react-router-dom';
import { ROUTE_PATHS } from '@/routes/paths';

interface LoginFormProps {
  onSubmit: (data: LoginData) => void;
  isLoading: boolean;
  error?: string | null;
}

export function LoginForm({ onSubmit, isLoading, error }: LoginFormProps) {
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<LoginData>({
    resolver: zodResolver(loginSchema),
    mode: 'onChange',
  });

  return (
    <FormContainer
      title="Login to your account"
      description="Enter your credentials to access your account."
      error={error}
    >
      <form onSubmit={handleSubmit(onSubmit)}>
        <FieldGroup>
          <FormField
            id="username"
            label="Username"
            type="text"
            placeholder="juandelacruz"
            error={errors?.username?.message}
            disabled={isLoading}
            register={register}
          />
          <FormField
            id="password"
            label="Password"
            type="password"
            placeholder="••••••••"
            error={errors?.password?.message}
            disabled={isLoading}
            register={register}
          />
          <FieldDescription className="text-xs pt-1 text-right">
            <Link to={ROUTE_PATHS.FORGOT_PASSWORD}>Forgot password?</Link>
          </FieldDescription>
          <Field>
            <Button
              type="submit"
              disabled={isLoading}
              className="w-full"
            >
              {isLoading ? 'Logging in...' : 'Login'}
            </Button>
            <FieldDescription className="text-center">
              Don't have an account? <Link to={ROUTE_PATHS.SIGNUP}>Sign up</Link>
            </FieldDescription>
          </Field>
        </FieldGroup>
      </form>
    </FormContainer>
  );
}
