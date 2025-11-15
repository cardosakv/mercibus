import { Card, CardHeader, CardTitle, CardDescription } from '@/components/ui/card';
import { MailCheckIcon } from '@/components/ui/icons/lucide-mail-check';

export function SignupEmailConfirm() {
  return (
    <Card>
      <CardHeader className="text-center mb-6">
        <div className="mx-auto mb-4 flex size-16 items-center justify-center rounded-full bg-primary/10">
          <MailCheckIcon className="size-8 text-primary" />
        </div>
        <CardTitle className="text-xl">Confirm Your Email</CardTitle>
        <CardDescription>
          We've sent a confirmation email to your inbox. Please check your email to complete the
          registration process.
        </CardDescription>
      </CardHeader>
    </Card>
  );
}
