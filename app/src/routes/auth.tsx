import { SignupPage } from '@/modules/auth/pages/signup-page';
import { ROUTE_PATHS } from './paths';
import type { RouteObject } from 'react-router-dom';
import { EmailSuccessPage } from '@/modules/auth/pages/email-success-page';
import { EmailErrorPage } from '@/modules/auth/pages/email-error-page';

export const authRoutes: RouteObject[] = [
  {
    path: ROUTE_PATHS.SIGNUP,
    element: <SignupPage />,
  },
  {
    path: ROUTE_PATHS.EMAIL_SUCCESS,
    element: <EmailSuccessPage />,
  },
  {
    path: ROUTE_PATHS.EMAIL_FAILURE,
    element: <EmailErrorPage />,
  },
];
