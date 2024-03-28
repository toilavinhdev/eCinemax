import { useFonts } from "expo-font";
import { Slot, SplashScreen, router } from "expo-router";
import React, { useEffect } from "react";
import { SafeAreaProvider } from "react-native-safe-area-context";
import { Provider } from "react-redux";
import store, { useAppDispatch, useAppSelector } from "~/features/store";
import { getMe } from "~/features/user";

SplashScreen.preventAutoHideAsync();

const RootLayout = () => {
  const [loaded, error] = useFonts({
    LexendDeca: require("../shared/assets/fonts/LexendDeca-VariableFont_wght.ttf"),
  });

  useEffect(() => {
    if (loaded) {
      SplashScreen.hideAsync();
    }
  }, [loaded]);

  if (!loaded) {
    return null;
  }

  return (
    <Provider store={store}>
      <SafeAreaProvider>
        <App />
      </SafeAreaProvider>
    </Provider>
  );
};

const App = () => {
  const dispatch = useAppDispatch();
  const loggedIn = useAppSelector((state) => state.user.loggedIn);

  useEffect(() => {
    if (!loggedIn) {
      router.replace("/auth");
    } else {
      router.replace("/");
      dispatch(getMe());
    }
  }, [loggedIn]);

  return <Slot />;
};

export default RootLayout;
