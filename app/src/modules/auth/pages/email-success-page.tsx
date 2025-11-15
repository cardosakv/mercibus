import { AuthLayout } from '@/components/layouts/auth-layout';
import { Button } from '@/components/ui/button';
import { Card, CardHeader, CardTitle, CardDescription, CardContent } from '@/components/ui/card';
import { CircleCheckIcon } from '@/components/ui/icons/lucide-circle-check';
import { ROUTE_PATHS } from '@/routes/paths';
import { Link } from 'react-router-dom';

export function EmailSuccessPage() {
  return (
    <>
      <title>Mercibus - Email Confirmation Successful</title>
      <AuthLayout>
        <Card>
          <CardHeader className="text-center mb-6">
            <div className="mx-auto mb-4 flex size-16 items-center justify-center rounded-full bg-green-100">
              <CircleCheckIcon className="size-8 text-green-600" />
            </div>
            <CardTitle className="text-xl">Registration Successful</CardTitle>
            <CardDescription>
              Your email has been successfully confirmed. You can now log in to your account.
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
    </>
  );
}
