import { useRouter } from "expo-router";

export const redirectToAuthScreen = () => {
  const router = useRouter();
  router.push("/auth");
};

export const redirectToMainScreen = () => {
  const router = useRouter();
  router.replace("/");
};
