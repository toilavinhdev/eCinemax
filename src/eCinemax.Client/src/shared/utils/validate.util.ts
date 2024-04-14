const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

export const isEmailValid = (email: string): boolean => {
  return emailRegex.test(email);
};

export const isEmptyOrWhitespace = (input: string): boolean => {
  return input === null || input.match(/^ *$/) !== null;
};
