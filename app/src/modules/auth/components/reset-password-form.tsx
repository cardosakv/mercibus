import { Button } from '@/components/ui/button';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card';
import { Field, FieldDescription, FieldGroup, FieldLabel } from '@/components/ui/field';
import { Input } from '@/components/ui/input';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { CircleAlertIcon } from '@/components/ui/icons/lucide-circle-alert';
import { Alert, AlertDescription } from '@/components/ui/alert';
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
    <Card>
      <CardHeader className="text-center">
        <CardTitle className="text-xl">Reset your password</CardTitle>
        <CardDescription>Enter your new password below.</CardDescription>
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
              <FieldLabel htmlFor="password">Password</FieldLabel>
              <div>
                <Input
                  id="password"
                  type="password"
                  placeholder="••••••••"
                  aria-invalid={!!errors?.password}
                  disabled={isLoading}
                  {...register('password')}
                />
                <FieldDescription className="text-destructive text-xs pt-1">
                  {errors?.password?.message}
                </FieldDescription>
              </div>
            </Field>
            <Field>
              <FieldLabel htmlFor="confirmPassword">Confirm Password</FieldLabel>
              <div>
                <Input
                  id="confirmPassword"
                  type="password"
                  placeholder="••••••••"
                  aria-invalid={!!errors?.confirmPassword}
                  disabled={isLoading}
                  {...register('confirmPassword')}
                />
                <FieldDescription className="text-destructive text-xs pt-1">
                  {errors?.confirmPassword?.message}
                </FieldDescription>
              </div>
            </Field>
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
      </CardContent>
    </Card>
  );
}
