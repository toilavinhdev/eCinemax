import { useAsyncStorage } from "@react-native-async-storage/async-storage";
import { Slot, router } from "expo-router";
import moment from "moment";
import React, { useEffect } from "react";
import { SafeAreaProvider } from "react-native-safe-area-context";
import { Provider } from "react-redux";
import { SpinnerOverlayComponent } from "~/core/components";
import store, { useAppDispatch, useAppSelector } from "~/features/store";
import { getMe, setAuthenticated } from "~/features/user";
import { authConst } from "~/shared/constants";

moment.locale("vi");

const RootLayout = () => {
  return (
    <Provider store={store}>
      <SafeAreaProvider>
        <AppGuard />
        <SpinnerOverlayComponent></SpinnerOverlayComponent>
      </SafeAreaProvider>
    </Provider>
  );
};

const AppGuard = () => {
  const dispatch = useAppDispatch();
  const authenticated = useAppSelector((state) => state.user.authenticated);
  const { getItem } = useAsyncStorage(authConst.ACCESS_TOKEN);

  useEffect(() => {
    const checkAuth = async () => {
      if (await getItem()) {
        dispatch(setAuthenticated(true));
        dispatch(getMe({}));
        router.replace("/");
      } else {
        dispatch(setAuthenticated(false));
        router.replace("/auth/sign-in");
      }
    };
    checkAuth().then();
  }, [authenticated]);

  return <Slot />;
};

export default RootLayout;
