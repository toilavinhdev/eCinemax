import { Stack } from "expo-router";
import React from "react";
import { StyleSheet } from "react-native";
import { colors } from "~/shared/constants";

const HomeLayout = () => {
  return (
    <Stack
      screenOptions={{
        title: "Home",
        headerStyle: styles.header,
      }}
    >
      <Stack.Screen name="index" />
      <Stack.Screen
        name="movie-detail"
        options={{
          title: "Movie detail",
          headerBackTitleVisible: false,
          headerTintColor: "white",
        }}
      />
      <Stack.Screen
        name="choose-cinema"
        options={{
          title: "Choose cinema",
          headerBackTitleVisible: false,
          headerTintColor: "white",
        }}
      />
      <Stack.Screen
        name="choose-seats"
        options={{
          title: "Choose seats",
          headerBackTitleVisible: false,
          headerTintColor: "white",
        }}
      />
      <Stack.Screen
        name="checkout"
        options={{
          title: "Checkout",
          headerBackTitleVisible: false,
          headerTintColor: "white",
        }}
      />
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
