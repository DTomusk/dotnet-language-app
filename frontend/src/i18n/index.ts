import i18n from 'i18next';
import { initReactI18next } from 'react-i18next';
import { fallbackLng, resources } from './resources';

// TODO: Determine the initial language based on user preferences or browser settings
const initialLanguage = fallbackLng;

void i18n.use(initReactI18next).init({
  resources,
  lng: initialLanguage,
  fallbackLng,
  interpolation: {
    escapeValue: false,
  },
});

export default i18n;
