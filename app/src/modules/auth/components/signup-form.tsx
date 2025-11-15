import { Button } from '@/components/ui/button';
import { FormContainer } from '@/components/ui/form-container';
import { FormField } from '@/components/ui/form-field';
import { Field, FieldDescription, FieldGroup } from '@/components/ui/field';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { signupSchema, type SignupData } from '../schemas/signup';
import { Link } from 'react-router-dom';
import { ROUTE_PATHS } from '@/routes/paths';

interface SignupFormProps {
  onSubmit: (data: SignupData) => void;
  isLoading: boolean;
  error?: string | null;
}

export function SignupForm({ onSubmit, isLoading, error }: SignupFormProps) {
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<SignupData>({
    resolver: zodResolver(signupSchema),
    mode: 'onChange',
  });

  return (
    <FormContainer
      title="Create your account"
      description="Enter your details to start shopping."
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
            id="email"
            label="Email"
            type="email"
            placeholder="user@mail.com"
            error={errors?.email?.message}
            disabled={isLoading}
            register={register}
          />
          <Field>
            <Field className="grid grid-cols-2 gap-4">
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
            </Field>
          </Field>
          <Field>
            <Button
              type="submit"
              disabled={isLoading}
              className="w-full"
            >
              {isLoading ? 'Creating...' : 'Create Account'}
            </Button>
            <FieldDescription className="text-center">
              Already have an account? <Link to={ROUTE_PATHS.LOGIN}>Sign in</Link>
            </FieldDescription>
          </Field>
        </FieldGroup>
      </form>
    </FormContainer>
  );
}
