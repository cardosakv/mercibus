import { z } from 'zod';

export const usernameSchema = z
  .string()
  .min(5, 'Username must be at least 5 characters.')
  .max(20, 'Username must not exceed 20 characters.')
  .regex(/^[a-zA-Z0-9_]+$/, 'Username can only contain letters, numbers, and underscores.');

export const passwordSchema = z
  .string()
  .min(8, 'Password must be at least 8 characters.')
  .regex(/^(?=.*[A-Z])/, 'Password must contain at least one uppercase letter.')
  .regex(/^(?=.*[a-z])/, 'Password must contain at least one lowercase letter.')
  .regex(/^(?=.*\d)/, 'Password must contain at least one number.')
  .regex(/^(?=.*[\W_])/, 'Password must contain at least one special character.');

export const confirmPasswordSchema = z.string();
