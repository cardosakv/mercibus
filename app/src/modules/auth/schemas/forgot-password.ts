import { z } from 'zod';
import { usernameSchema } from './shared';

export const forgotPasswordSchema = z.object({
  username: usernameSchema,
});

export type ForgotPasswordData = z.infer<typeof forgotPasswordSchema>;
