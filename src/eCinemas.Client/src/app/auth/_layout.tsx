import { Stack } from "expo-router";
import React from "react";

const AuthLayout = () => {
  return (
    <Stack initialRouteName="sign-in" screenOptions={{ headerShown: false }}>
      <Stack.Screen name="sign-in" />
      <Stack.Screen name="sign-up" />
    </Stack>
  );
};

export default AuthLayout;
