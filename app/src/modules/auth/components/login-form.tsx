import { Button } from '@/components/ui/button';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card';
import { Field, FieldDescription, FieldGroup, FieldLabel } from '@/components/ui/field';
import { Input } from '@/components/ui/input';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { CircleAlertIcon } from '@/components/ui/icons/lucide-circle-alert';
import { Alert, AlertDescription } from '@/components/ui/alert';
import { loginSchema, type LoginData } from '../schemas/login';
import { Link } from 'react-router-dom';

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
    <Card>
      <CardHeader className="text-center">
        <CardTitle className="text-xl">Login to your account</CardTitle>
        <CardDescription>Enter your credentials to access your account.</CardDescription>
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
              <Button
                type="submit"
                disabled={isLoading}
                className="w-full"
              >
                {isLoading ? 'Logging in...' : 'Login'}
              </Button>
              <FieldDescription className="text-center">
                Not registered yet? <Link to="/signup">Sign up</Link>
              </FieldDescription>
            </Field>
          </FieldGroup>
        </form>
      </CardContent>
    </Card>
  );
}
