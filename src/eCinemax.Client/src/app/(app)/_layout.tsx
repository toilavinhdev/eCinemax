import { Stack } from "expo-router";
import { useEffect } from "react";
import { useAppDispatch } from "~/features/store";
import { getMe } from "~/features/user";

const AppLayout = () => {
  const dispatch = useAppDispatch();

  useEffect(() => {
    dispatch(getMe({}));
  }, []);

  return <Stack screenOptions={{ headerShown: false }} />;
};

export default AppLayout;
