import { getLocales } from "expo-localization";
import { I18n } from "i18n-js";

const translations = {
  en: require("./translations/en.json"),
  vi: require("./translations/vi.json"),
};

export const I18nManager = (locale?: string): I18n => {
  // Set the key-value pairs for the different languages you want to support.
  const i18n = new I18n(translations);

  // Set the locale tag once at the beginning of your app.
  i18n.locale = locale ?? getLocales()[0].languageCode!;

  // When a value is missing from a language it'll fall back to another language with the key present.
  i18n.enableFallback = true;
  // To see the fallback mechanism uncomment the line below to force the app to use the Japanese language.

  return i18n;
};
