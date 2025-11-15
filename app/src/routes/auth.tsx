import { SignupPage } from '@/modules/auth/pages/signup-page';
import { SignupSuccessPage } from '@/modules/auth/pages/signup-success-page';
import type { RouteObject } from 'react-router-dom';
import { ROUTE_PATHS } from './paths';

export const authRoutes: RouteObject[] = [
  {
    path: ROUTE_PATHS.SIGNUP,
    element: <SignupPage />,
  },
  {
    path: ROUTE_PATHS.SIGNUP_SUCCESS,
    element: <SignupSuccessPage />,
  },
];
