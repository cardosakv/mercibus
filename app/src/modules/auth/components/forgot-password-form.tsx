import { Button } from '@/components/ui/button';
import { FormContainer } from '@/components/ui/form-container';
import { FormField } from '@/components/ui/form-field';
import { Field, FieldDescription, FieldGroup } from '@/components/ui/field';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { Link } from 'react-router-dom';
import { forgotPasswordSchema, type ForgotPasswordData } from '../schemas/forgot-password';
import { ROUTE_PATHS } from '@/routes/paths';

interface ForgotPasswordFormProps {
  onSubmit: (data: ForgotPasswordData) => void;
  isLoading: boolean;
  error?: string | null;
}

export function ForgotPasswordForm({ onSubmit, isLoading, error }: ForgotPasswordFormProps) {
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<ForgotPasswordData>({
    resolver: zodResolver(forgotPasswordSchema),
    mode: 'onChange',
  });

  return (
    <FormContainer
      title="Forgot your password?"
      description="Enter your username to reset your password."
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
          <Field>
            <Button
              type="submit"
              disabled={isLoading}
              className="w-full"
            >
              {isLoading ? 'Sending...' : 'Send Reset Link'}
            </Button>
            <FieldDescription className="text-center">
              Remembered your password? <Link to={ROUTE_PATHS.LOGIN}>Login</Link>
            </FieldDescription>
          </Field>
        </FieldGroup>
      </form>
    </FormContainer>
  );
}
