import { router } from "expo-router";
import { useEffect } from "react";
import { useAppDispatch, useAppSelector } from "~/features/store";
import { getMe } from "~/features/user/user.thunk";

export function useAuthGuard() {
  const isAuthorized = useAppSelector((state) => state.user.isAuthorized);
  const dispatch = useAppDispatch();
  const user = useAppSelector((state) => state.user.currentUser);

  useEffect(() => {
    if (!isAuthorized) {
      router.replace("/auth");
    } else {
      if (!user) dispatch(getMe());
      router.replace("/");
    }
  }, [isAuthorized]);
}
