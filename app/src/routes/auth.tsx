import { SignupPage } from '@/modules/auth/pages/signup-page';
import type { RouteObject } from 'react-router-dom';

export const authRoutes: RouteObject = {
  path: '/signup',
  element: <SignupPage />,
};
