import logo from '@/assets/images/mercibus.png';
import { Button } from '../ui/button';
import { authService } from '@/modules/auth/api/service';
import { getErrorMessage } from '@/utils/error';

export function MainLayout() {
  const handleGetUserInfo = async () => {
    try {
      const response = await authService.getUserInfo();
      console.log('User Info:', response);
    } catch (error) {
      const errorMessage = getErrorMessage(error);
      console.error('Error fetching user info:', errorMessage);
    }
  };

  return (
    <div className="bg-muted flex min-h-svh flex-col items-center justify-center p-12">
      <div className="flex w-full max-w-md flex-col gap-6">
        <a
          href="/"
          className="flex items-center gap-2 self-center font-bold text-primary"
        >
          <div className="flex size-6 items-center justify-center rounded-md">
            <img
              src={logo}
              alt="Mercibus Logo"
            />
          </div>
          Mercibus Home
        </a>
        <Button onClick={handleGetUserInfo}>Get User Info</Button>
      </div>
    </div>
  );
}
