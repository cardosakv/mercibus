import { Field, FieldDescription, FieldLabel } from '@/components/ui/field';
import { Input } from '@/components/ui/input';
import { type ReactNode } from 'react';

interface FormFieldProps {
  id: string;
  label: string;
  type?: string;
  placeholder?: string;
  error?: string;
  disabled?: boolean;
  children?: ReactNode;
  register?: any;
  description?: string;
}

export function FormField({
  id,
  label,
  type = 'text',
  placeholder,
  error,
  disabled,
  children,
  register,
  description,
}: FormFieldProps) {
  return (
    <Field>
      <FieldLabel htmlFor={id}>{label}</FieldLabel>
      <div>
        {children ? (
          children
        ) : (
          <Input
            id={id}
            type={type}
            placeholder={placeholder}
            aria-invalid={!!error}
            disabled={disabled}
            {...(register ? register(id) : {})}
          />
        )}
        {error && (
          <FieldDescription className="text-destructive text-xs pt-1">{error}</FieldDescription>
        )}
        {description && !error && (
          <FieldDescription className="text-xs pt-1">{description}</FieldDescription>
        )}
      </div>
    </Field>
  );
}
