import { Button } from '@/components/ui/button';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card';
import { Field, FieldDescription, FieldGroup, FieldLabel } from '@/components/ui/field';
import { Input } from '@/components/ui/input';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { signupSchema, type SignupData } from '../schemas/signup';
import { CircleAlertIcon } from '@/components/ui/icons/lucide-circle-alert';
import { Alert, AlertDescription } from '@/components/ui/alert';

interface SignupFormProps {
  onSubmit: (data: SignupData) => void;
  isLoading: boolean;
}

export function SignupForm({ onSubmit, isLoading }: SignupFormProps) {
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<SignupData>({
    resolver: zodResolver(signupSchema),
    mode: 'onChange',
  });

  return (
    <div className="flex flex-col">
      <Card>
        <CardHeader className="text-center">
          <CardTitle className="text-xl">Create your account</CardTitle>
          <CardDescription>Enter your details to start shopping.</CardDescription>
        </CardHeader>
        <CardContent>
          <form onSubmit={handleSubmit(onSubmit)}>
            <FieldGroup>
              <Alert
                variant="destructive"
                className="bg-destructive/10 border-destructive text-destructive"
              >
                <CircleAlertIcon />
                <AlertDescription>
                  Username is already registered. Please try to use a different one.
                </AlertDescription>
              </Alert>
              <Field>
                <FieldLabel htmlFor="username">Username</FieldLabel>
                <div>
                  <Input
                    id="username"
                    type="text"
                    placeholder="juandelacruz"
                    disabled={isLoading}
                    {...register('username')}
                  />
                  <FieldDescription className="text-destructive text-xs pt-1">
                    {errors?.username?.message}
                  </FieldDescription>
                </div>
              </Field>
              <Field>
                <FieldLabel htmlFor="email">Email</FieldLabel>
                <div>
                  <Input
                    id="email"
                    type="email"
                    placeholder="user@mail.com"
                    disabled={isLoading}
                    {...register('email')}
                  />
                  <FieldDescription className="text-destructive text-xs pt-1">
                    {errors?.email?.message}
                  </FieldDescription>
                </div>
              </Field>
              <Field>
                <Field className="grid grid-cols-2 gap-4">
                  <Field>
                    <FieldLabel htmlFor="password">Password</FieldLabel>
                    <div>
                      <Input
                        id="password"
                        type="password"
                        placeholder="••••••••"
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
                        disabled={isLoading}
                        {...register('confirmPassword')}
                      />
                      <FieldDescription className="text-destructive text-xs pt-1 min-h-1">
                        {errors?.confirmPassword?.message}
                      </FieldDescription>
                    </div>
                  </Field>
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
                  Already have an account? <a href="#">Sign in</a>
                </FieldDescription>
              </Field>
            </FieldGroup>
          </form>
        </CardContent>
      </Card>
    </div>
  );
}
