const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

export const isEmailValid = (email: string): boolean => {
  return emailRegex.test(email);
};
