import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card';
import { Alert, AlertDescription } from '@/components/ui/alert';
import { CircleAlertIcon } from '@/components/ui/icons/lucide-circle-alert';
import type { ReactNode } from 'react';

interface FormContainerProps {
  title: string;
  description: string;
  error?: string | null;
  children: ReactNode;
}

export function FormContainer({ title, description, error, children }: FormContainerProps) {
  return (
    <Card>
      <CardHeader className="text-center">
        <CardTitle className="text-xl">{title}</CardTitle>
        <CardDescription>{description}</CardDescription>
      </CardHeader>
      <CardContent>
        {error && (
          <Alert
            variant="destructive"
            className="bg-destructive/10 border-destructive text-destructive mb-6"
          >
            <CircleAlertIcon />
            <AlertDescription>{error}</AlertDescription>
          </Alert>
        )}
        {children}
      </CardContent>
    </Card>
  );
}
