import { Button } from '@/components/ui/button';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card';
import { Field, FieldDescription, FieldGroup, FieldLabel } from '@/components/ui/field';
import { Input } from '@/components/ui/input';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { CircleAlertIcon } from '@/components/ui/icons/lucide-circle-alert';
import { Alert, AlertDescription } from '@/components/ui/alert';
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
    <Card>
      <CardHeader className="text-center">
        <CardTitle className="text-xl">Forgot your password?</CardTitle>
        <CardDescription>Enter your username to reset your password.</CardDescription>
      </CardHeader>
      <CardContent>
        <form onSubmit={handleSubmit(onSubmit)}>
          <FieldGroup>
            {error && (
              <Alert
                variant="destructive"
                className="bg-destructive/10 border-destructive text-destructive"
              >
                <CircleAlertIcon />
                <AlertDescription>{error}</AlertDescription>
              </Alert>
            )}
            <Field>
              <FieldLabel htmlFor="username">Username</FieldLabel>
              <div>
                <Input
                  id="username"
                  type="text"
                  placeholder="juandelacruz"
                  aria-invalid={!!errors?.username}
                  disabled={isLoading}
                  {...register('username')}
                />
                <FieldDescription className="text-destructive text-xs pt-1">
                  {errors?.username?.message}
                </FieldDescription>
              </div>
            </Field>
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
      </CardContent>
    </Card>
  );
}
