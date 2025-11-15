import { SignupPage } from '@/modules/auth/pages/signup-page';
import { ROUTE_PATHS } from './paths';
import type { RouteObject } from 'react-router-dom';

export const authRoutes: RouteObject[] = [
  {
    path: ROUTE_PATHS.SIGNUP,
    element: <SignupPage />,
  },
];
