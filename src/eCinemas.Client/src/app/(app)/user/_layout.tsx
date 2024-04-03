import { Stack } from "expo-router";
import React from "react";

const UserLayout = () => {
  return (
    <Stack>
      <Stack.Screen
        name="update-password"
        options={{ title: "Thay đổi mật khẩu" }}
      />
      <Stack.Screen
        name="update-profile"
        options={{ title: "Hồ sơ của bạn" }}
      />
    </Stack>
  );
};

export default UserLayout;
