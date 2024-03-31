import {
  Entypo,
  FontAwesome,
  MaterialCommunityIcons,
  MaterialIcons,
} from "@expo/vector-icons";
import { Tabs } from "expo-router";
import React from "react";
import { colors } from "~/shared/constants";

const MainLayout = () => {
  return (
    <Tabs
      screenOptions={{
        headerTintColor: "white",
        headerShadowVisible: false,
        headerStyle: {
          backgroundColor: colors.secondary,
        },
        tabBarStyle: {
          backgroundColor: colors.secondary,
          borderTopWidth: 0,
        },
        tabBarActiveTintColor: colors.primary,
      }}
    >
      <Tabs.Screen
        name="index"
        options={{
          title: "Trang chủ",
          tabBarIcon: ({ focused }) => (
            <Entypo
              name="home"
              size={22}
              color={focused ? colors.primary : colors.gray}
            />
          ),
        }}
      />
      <Tabs.Screen
        name="notification"
        options={{
          title: "Thông báo",
          tabBarIcon: ({ focused }) => (
            <MaterialCommunityIcons
              name="bell"
              size={22}
              color={focused ? colors.primary : colors.gray}
            />
          ),
        }}
      />
      <Tabs.Screen
        name="voucher"
        options={{
          title: "Khuyến mại",
          tabBarIcon: ({ focused }) => (
            <MaterialIcons
              name="discount"
              size={19}
              color={focused ? colors.primary : colors.gray}
            />
          ),
        }}
      ></Tabs.Screen>
      <Tabs.Screen
        name="other"
        options={{
          title: "Khác",
          tabBarIcon: ({ focused }) => (
            <FontAwesome
              name="bars"
              size={22}
              color={focused ? colors.primary : colors.gray}
            />
          ),
        }}
      ></Tabs.Screen>
    </Tabs>
  );
};

export default MainLayout;
