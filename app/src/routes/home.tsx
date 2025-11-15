import type { RouteObject } from 'react-router-dom';
import { ROUTE_PATHS } from './paths';
import { MainLayout } from '@/components/layouts/main-layout';

export const homeRoutes: RouteObject[] = [
  {
    path: ROUTE_PATHS.HOME,
    element: <MainLayout />,
  },
];
