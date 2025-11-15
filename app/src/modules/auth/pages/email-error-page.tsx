import { AuthLayout } from '@/components/layouts/auth-layout';
import { Button } from '@/components/ui/button';
import { Card, CardHeader, CardTitle, CardDescription, CardContent } from '@/components/ui/card';
import { CircleXIcon } from '@/components/ui/icons/lucide-circle-x';
import { ROUTE_PATHS } from '@/routes/paths';
import { Link } from 'react-router-dom';

export function EmailErrorPage() {
  return (
    <AuthLayout>
      <Card>
        <CardHeader className="text-center mb-6">
          <div className="mx-auto mb-4 flex size-16 items-center justify-center rounded-full bg-destructive/10">
            <CircleXIcon className="size-8 text-destructive" />
          </div>
          <CardTitle className="text-xl">Registration Failed</CardTitle>
          <CardDescription>
            There was an error confirming your email. Please try again or contact support.
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
    </AuthLayout>
  );
}
