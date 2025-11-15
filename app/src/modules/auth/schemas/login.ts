import { z } from 'zod';
import { usernameSchema, passwordSchema } from './shared';

export const loginSchema = z.object({
  username: usernameSchema,
  password: passwordSchema,
});

export type LoginData = z.infer<typeof loginSchema>;
