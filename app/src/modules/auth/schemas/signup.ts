import { z } from 'zod';

export const signupSchema = z
  .object({
    username: z
      .string()
      .min(5, 'Username must be at least 5 characters.')
      .max(20, 'Username must not exceed 20 characters.')
      .regex(/^[a-zA-Z0-9_]+$/, 'Username can only contain letters, numbers, and underscores.'),

    email: z.email('Invalid email address.'),

    password: z
      .string()
      .min(8, 'Password must be at least 8 characters.')
      .regex(/^(?=.*[A-Z])/, 'Password must contain at least one uppercase letter.')
      .regex(/^(?=.*[a-z])/, 'Password must contain at least one lowercase letter.')
      .regex(/^(?=.*\d)/, 'Password must contain at least one number.')
      .regex(/^(?=.*[\W_])/, 'Password must contain at least one special character.'),

    confirmPassword: z.string(),
  })
  .refine((data) => data.password === data.confirmPassword, {
    message: 'Passwords do not match.',
    path: ['confirmPassword'],
  });

export type SignupData = z.infer<typeof signupSchema>;
