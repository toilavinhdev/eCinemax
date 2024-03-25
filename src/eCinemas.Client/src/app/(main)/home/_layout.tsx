import { Stack } from "expo-router";
import React from "react";
import { StyleSheet } from "react-native";
import { colors } from "~/shared/constants";

const HomeLayout = () => {
  return (
    <Stack
      screenOptions={{
        title: "eCinemas",
        headerStyle: styles.header,
        headerTitleStyle: styles.title,
        headerTitleAlign: "left",
      }}
    >
      <Stack.Screen name="index" />
      <Stack.Screen name="[id]" />
    </Stack>
  );
};

const styles = StyleSheet.create({
  header: {
    backgroundColor: colors.secondary,
  },
  title: {
    color: "white",
  },
});

export default HomeLayout;
