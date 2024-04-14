import { Stack } from "expo-router";
import React from "react";
import { colors } from "~/shared/constants";

const CollectionLayout = () => {
  return (
    <Stack
      screenOptions={{
        headerStyle: { backgroundColor: colors.secondary },
        headerTintColor: "white",
      }}
    >
      <Stack.Screen name="index" options={{ title: "Phim đã lưu" }} />
    </Stack>
  );
};

export default CollectionLayout;
