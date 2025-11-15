import { Button } from '@/components/ui/button';
import { FormContainer } from '@/components/ui/form-container';
import { FormField } from '@/components/ui/form-field';
import { Field, FieldDescription, FieldGroup } from '@/components/ui/field';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { Link } from 'react-router-dom';
import { ROUTE_PATHS } from '@/routes/paths';
import { resetPasswordSchema, type ResetPasswordData } from '../schemas/reset-password';

interface ResetPasswordFormProps {
  onSubmit: (data: ResetPasswordData) => void;
  isLoading: boolean;
  error?: string | null;
}

export function ResetPasswordForm({ onSubmit, isLoading, error }: ResetPasswordFormProps) {
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<ResetPasswordData>({
    resolver: zodResolver(resetPasswordSchema),
    mode: 'onChange',
  });

  return (
    <FormContainer
      title="Reset your password"
      description="Enter your new password below."
      error={error}
    >
      <form onSubmit={handleSubmit(onSubmit)}>
        <FieldGroup>
          <FormField
            id="password"
            label="Password"
            type="password"
            placeholder="••••••••"
            error={errors?.password?.message}
            disabled={isLoading}
            register={register}
          />
          <FormField
            id="confirmPassword"
            label="Confirm Password"
            type="password"
            placeholder="••••••••"
            error={errors?.confirmPassword?.message}
            disabled={isLoading}
            register={register}
          />
          <Field>
            <Button
              type="submit"
              disabled={isLoading}
              className="w-full"
            >
              {isLoading ? 'Resetting...' : 'Reset Password'}
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
