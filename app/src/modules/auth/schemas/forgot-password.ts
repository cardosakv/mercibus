import { z } from 'zod';

export const forgotPasswordSchema = z.object({
  username: z
    .string()
    .min(5, 'Username must be at least 5 characters.')
    .max(20, 'Username must not exceed 20 characters.')
    .regex(/^[a-zA-Z0-9_]+$/, 'Username can only contain letters, numbers, and underscores.'),
});

export type ForgotPasswordData = z.infer<typeof forgotPasswordSchema>;
