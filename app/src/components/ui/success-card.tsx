import { CircleCheckIcon } from '@/components/ui/icons/lucide-circle-check';
import type { ReactNode } from 'react';
import { Card, CardHeader, CardTitle, CardDescription, CardContent } from './card';

interface SuccessCardProps {
  title: string;
  description: string;
  action?: ReactNode;
}

export function SuccessCard({ title, description, action }: SuccessCardProps) {
  return (
    <Card>
      <CardHeader className="text-center mb-6">
        <div className="mx-auto mb-4 flex size-16 items-center justify-center rounded-full bg-green-100">
          <CircleCheckIcon className="size-8 text-green-600" />
        </div>
        <CardTitle className="text-xl">{title}</CardTitle>
        <CardDescription>{description}</CardDescription>
      </CardHeader>
      {action && (
        <CardContent>
          <div className="flex flex-col">{action}</div>
        </CardContent>
      )}
    </Card>
  );
}
