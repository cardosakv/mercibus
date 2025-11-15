import { Button } from '@/components/ui/button';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card';
import { CheckCircleIcon } from 'lucide-react';
import { ROUTE_PATHS } from '@/routes/paths';
import { Link } from 'react-router-dom';
import { AuthLayout } from '@/components/layouts/auth-layout';

export function SignupSuccessPage() {
  return (
    <AuthLayout>
      <Card>
        <CardHeader className="text-center mb-6">
          <div className="mx-auto mb-4 flex size-16 items-center justify-center rounded-full bg-green-100">
            <CheckCircleIcon className="size-8 text-green-600" />
          </div>
          <CardTitle className="text-xl">Registration Successful!</CardTitle>
          <CardDescription>
            We've sent a confirmation email to your inbox. Please check your email to complete the
            registration process.
          </CardDescription>
        </CardHeader>
        <CardContent>
          <div className="flex flex-col">
            <Button asChild>
              <Link to={ROUTE_PATHS.LOGIN}>Back to Sign In</Link>
            </Button>
          </div>
        </CardContent>
      </Card>
    </AuthLayout>
  );
}
