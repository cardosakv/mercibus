export const errorMap: Record<string, string> = {
  validation_failed: 'One or more validation errors occurred.',

  user_id_required: 'User ID is required.',
  username_required: 'Username is required.',
  username_invalid: 'Username contains invalid characters.',
  username_too_short: 'Username is too short.',
  username_too_long: 'Username is too long.',
  username_already_exists: 'Username is already taken.',

  name_too_short: 'Name is too short.',
  name_too_long: 'Name is too long.',

  email_required: 'Email is required.',
  email_invalid: 'Invalid email format.',
  email_already_exists: 'Email is already registered.',
  email_already_verified: 'Email is already verified.',

  password_required: 'Password is required.',
  password_too_short: 'Password is too short.',
  password_invalid: 'Password does not meet requirements.',
  password_mismatch: 'Passwords do not match.',
  password_already_set: 'Password has already been set.',

  role_invalid: 'Invalid role.',
  role_already_exists: 'Role already exists.',

  user_not_found: 'User not found.',
  user_no_role_assigned: 'User has no role assigned.',
  user_already_in_role: 'User is already in this role.',

  login_already_associated: 'This login is already associated with another account.',
  token_required: 'A token is required.',
  token_invalid: 'The token is invalid.',
  refresh_token_expired: 'The refresh token has expired.',
  user_locked: 'User account is locked.',

  unauthorized: 'You are not authorized to perform this action.',
  internal: 'Something went wrong. Please try again later.',
};
