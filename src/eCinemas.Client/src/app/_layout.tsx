import AsyncStorage from "@react-native-async-storage/async-storage";
import { Slot, router } from "expo-router";
import React, { useEffect } from "react";
import { SafeAreaProvider } from "react-native-safe-area-context";
import { Provider } from "react-redux";
import store, { useAppDispatch } from "~/features/store";
import { getMe } from "~/features/user";
import { authConst } from "~/shared/constants";

const RootLayout = () => {
  return (
    <Provider store={store}>
      <SafeAreaProvider>
        <AppGuard />
      </SafeAreaProvider>
    </Provider>
  );
};

const AppGuard = () => {
  const dispatch = useAppDispatch();

  const handle = async () => {
    const accessToken = await AsyncStorage.getItem(authConst.ACCESS_TOKEN);

    if (!accessToken) {
      router.replace("/auth/sign-in");
    } else {
      dispatch(getMe());
    }
  };

  useEffect(() => {
    handle();
  });

  return <Slot />;
};

export default RootLayout;
