import { Slot } from "expo-router";
import moment from "moment";
import React from "react";
import { SafeAreaProvider } from "react-native-safe-area-context";
import { Provider } from "react-redux";
import store from "~/features/store";
import { SpinnerOverlayComponent } from "~/shared/components";

moment.locale("vi");

const RootLayout = () => {
  return (
    <Provider store={store}>
      <SafeAreaProvider>
        <Slot />
        <SpinnerOverlayComponent></SpinnerOverlayComponent>
      </SafeAreaProvider>
    </Provider>
  );
};

export default RootLayout;
