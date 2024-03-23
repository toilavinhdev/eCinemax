import { Slot, SplashScreen } from "expo-router";
import React, { useEffect } from "react";
import { SafeAreaProvider } from "react-native-safe-area-context";
import { Provider } from "react-redux";
import { useAuthGuard } from "~/core/guards";
import store from "~/features/store";

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
  useAuthGuard();
  return <Slot />;
};

export default RootLayout;
