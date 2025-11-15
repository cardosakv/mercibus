import { SignupPage } from '@/modules/auth/pages/signup-page';
import { ROUTE_PATHS } from './paths';
import type { RouteObject } from 'react-router-dom';
import { EmailSuccessPage } from '@/modules/auth/pages/email-success-page';
import { EmailErrorPage } from '@/modules/auth/pages/email-error-page';
import { LoginPage } from '@/modules/auth/pages/login-page';
import { ForgotPasswordPage } from '@/modules/auth/pages/forgot-password-page';
import { ResetPasswordPage } from '@/modules/auth/pages/reset-password-page';

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
  {
    path: ROUTE_PATHS.LOGIN,
    element: <LoginPage />,
  },
  {
    path: ROUTE_PATHS.FORGOT_PASSWORD,
    element: <ForgotPasswordPage />,
  },
  {
    path: ROUTE_PATHS.RESET_PASSWORD,
    element: <ResetPasswordPage />,
  },
];
