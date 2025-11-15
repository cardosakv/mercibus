import { z } from 'zod';
import { usernameSchema, passwordSchema, confirmPasswordSchema } from './shared';

export const signupSchema = z
  .object({
    username: usernameSchema,
    email: z.email('Invalid email address.'),
    password: passwordSchema,
    confirmPassword: confirmPasswordSchema,
  })
  .refine((data) => data.password === data.confirmPassword, {
    message: 'Passwords do not match.',
    path: ['confirmPassword'],
  });

export type SignupData = z.infer<typeof signupSchema>;
