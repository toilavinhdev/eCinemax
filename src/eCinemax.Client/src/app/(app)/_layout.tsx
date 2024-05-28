import { Redirect, Stack } from "expo-router";
import { useEffect, useState } from "react";
import { useAppDispatch, useAppSelector } from "~/features/store";
import { getMe } from "~/features/user";
import SplashComponent from "~/shared/components/splash/SplashComponent";

const AppLayout = () => {
  const dispatch = useAppDispatch();
  const { currentUser, status } = useAppSelector((state) => state.user);
  const [appReady, setAppReady] = useState<boolean>(false);

  useEffect(() => {
    dispatch(getMe({}));
  }, []);

  useEffect(() => {
    console.log("App Status", status);
    setTimeout(() => {
      setAppReady(status !== "idle" || status !== ("loading" as string));
    }, 1800);
  }, [status]);

  if (!currentUser && appReady) return <Redirect href="/auth/sign-in" />;

  return appReady ? (
    <Stack screenOptions={{ headerShown: false }} />
  ) : (
    <SplashComponent />
  );
};

export default AppLayout;
