import { Slot, SplashScreen, router } from "expo-router";
import React, { useEffect } from "react";
import { SafeAreaProvider } from "react-native-safe-area-context";
import { Provider } from "react-redux";
import store, { useAppDispatch, useAppSelector } from "~/features/store";
import { getMe } from "~/features/user/user.thunk";

SplashScreen.preventAutoHideAsync();

const RootLayout = () => {
  useEffect(() => {
    setTimeout(() => {
      SplashScreen.hideAsync();
    }, 3000);
  }, []);

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
  const isAuthorized = useAppSelector((state) => state.user.isAuthorized);

  useEffect(() => {
    if (!isAuthorized) {
      router.replace("/auth");
    } else {
      router.replace("/");
      dispatch(getMe());
    }
  }, [isAuthorized]);

  return <Slot />;
};

export default RootLayout;
