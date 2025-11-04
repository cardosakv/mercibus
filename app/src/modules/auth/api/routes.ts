export const AUTH_API = {
  REGISTER: '/auth/register',
  LOGIN: '/auth/login',
  LOGOUT: '/auth/logout',
  REFRESH_TOKEN: '/auth/refresh-token',
  SEND_CONFIRMATION_EMAIL: '/auth/send-confirmation-email',
  CONFIRM_EMAIL: '/auth/confirm-email',
  FORGOT_PASSWORD: '/auth/forgot-password',
  RESET_PASSWORD: '/auth/reset-password',
  INFO: '/auth/info',
  UPLOAD_PROFILE_PICTURE: '/auth/upload-profile-picture',
} as const;
