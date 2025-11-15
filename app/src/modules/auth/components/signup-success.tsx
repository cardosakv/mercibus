import { Button } from '@/components/ui/button';
import { Card, CardHeader, CardTitle, CardDescription, CardContent } from '@/components/ui/card';
import { ROUTE_PATHS } from '@/routes/paths';
import { Link } from 'react-router-dom';
import { CheckCircleIcon } from '@/components/ui/icons/heroicons-check-circle';

export function SignupSuccess() {
  return (
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
            <Link to={ROUTE_PATHS.LOGIN}>Go to Login</Link>
          </Button>
        </div>
      </CardContent>
    </Card>
  );
}
