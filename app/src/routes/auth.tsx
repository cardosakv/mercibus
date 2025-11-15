import { SignupPage } from '@/modules/auth/pages/signup-page';
import { RegisterSuccessPage } from '@/modules/auth/pages/register-success-page';
import type { RouteObject } from 'react-router-dom';
import { ROUTE_PATHS } from './paths';

export const authRoutes: RouteObject[] = [
  {
    path: ROUTE_PATHS.SIGNUP,
    element: <SignupPage />,
  },
  {
    path: ROUTE_PATHS.SIGNUP_SUCCESS,
    element: <RegisterSuccessPage />,
  },
];
