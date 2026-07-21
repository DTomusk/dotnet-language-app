export const defaultNS = 'common';
export const fallbackLng = 'en';

export const resources = {
  en: {
    common: {
      actions: {
        submitting: 'Submitting...',
        submit: 'Submit',
      },
      errors: {
        genericSubmit: 'Something went wrong. Please try again.',
      },
    },
    auth: {
      title: {
        login: 'Yo, whaddup😎',
        register: 'Howdy 🤠💅',
      },
      subtitle: {
        login: 'Sign in now to make your dreams come ✨true✨',
        register: 'Sign up now!',
      },
      fields: {
        username: 'Username',
        password: 'Password',
        confirmPassword: 'Confirm password',
      },
      actions: {
        submitLogin: 'Sign in',
        submitRegister: 'Create account',
        switchToLogin: 'Sign in',
        switchToRegister: 'Why not make one?',
      },
      prompts: {
        alreadyHaveAccount: 'Already have an account? ',
        newHere: 'Don\'t have an account?👀 ',
      },
      validation: {
        usernameRequired: 'Username is required.',
        usernameMinLength: 'Username must be at least 8 characters.',
        passwordRequired: 'Password is required.',
        passwordMinLength: 'Password must be at least 8 characters.',
        confirmPasswordRequired: 'Please confirm your password.',
        passwordsDoNotMatch: 'Passwords do not match.',
      },
    },
    languagePractice: {
      submission: {
        Title: 'Submit your text',
        fields: {
          text: 'Text',
        },
        validation: {
          textRequired: 'Submission text is required.',
          textMaxLength: 'Submission text must be at most 500 characters.',
        },
      },
    },
  },
} as const;
