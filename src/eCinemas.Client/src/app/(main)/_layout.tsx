import {
  Entypo,
  FontAwesome,
  MaterialCommunityIcons,
  MaterialIcons,
} from "@expo/vector-icons";
import { Tabs } from "expo-router";
import React from "react";
import { StyleSheet } from "react-native";
import { colors } from "~/shared/constants";

const MainLayout = () => {
  return (
    <Tabs
      screenOptions={{
        tabBarStyle: styles.tabBar,
        tabBarActiveTintColor: colors.primary,
      }}
    >
      <Tabs.Screen redirect name="index" />
      <Tabs.Screen
        name="home"
        options={{
          title: "Home",
          headerShown: false,
          tabBarIcon: ({ focused }) => (
            <Entypo
              name="home"
              size={28}
              color={focused ? colors.primary : colors.gray}
            />
          ),
        }}
      />
      <Tabs.Screen
        name="news"
        options={{
          title: "Notifications",
          tabBarIcon: ({ focused }) => (
            <MaterialCommunityIcons
              name="bell"
              size={28}
              color={focused ? colors.primary : colors.gray}
            />
          ),
        }}
      />
      <Tabs.Screen
        name="voucher"
        options={{
          title: "Voucher",
          tabBarIcon: ({ focused }) => (
            <MaterialIcons
              name="discount"
              size={24}
              color={focused ? colors.primary : colors.gray}
            />
          ),
        }}
      ></Tabs.Screen>
      <Tabs.Screen
        name="profile"
        options={{
          title: "User",
          headerShown: false,
          tabBarIcon: ({ focused }) => (
            <FontAwesome
              name="user"
              size={25}
              color={focused ? colors.primary : colors.gray}
            />
          ),
        }}
      ></Tabs.Screen>
    </Tabs>
  );
};

export default MainLayout;

const styles = StyleSheet.create({
  tabBar: { backgroundColor: colors.secondary, borderTopWidth: 0 },
});
