import { createBrowserRouter } from 'react-router-dom';
import { authRoutes } from './auth';
import { homeRoutes } from './home';

export const router = createBrowserRouter([...homeRoutes, ...authRoutes]);
