import {
  Entypo,
  FontAwesome,
  MaterialCommunityIcons,
} from "@expo/vector-icons";
import { Tabs } from "expo-router";
import React from "react";
import { Text, View } from "react-native";
import { useSafeAreaInsets } from "react-native-safe-area-context";
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
          header: () => <HomeHeaderComponent />,
        }}
      />
      <Tabs.Screen
        name="cinema"
        options={{
          title: "Rạp chiếu",
          headerTitle: "Rạp chiếu gần bạn",
          tabBarIcon: ({ focused }) => (
            <Entypo
              name="location-pin"
              size={24}
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

const HomeHeaderComponent = () => {
  const insets = useSafeAreaInsets();

  return (
    <View
      className="h-[60px] px-3 flex-row justify-center items-center"
      style={{
        paddingTop: insets.top,
        height: insets.top + 60,
        backgroundColor: colors.secondary,
      }}
    >
      <View>
        <Text
          className="font-[900] text-[24px]"
          style={{ color: colors.primary }}
        >
          CINEMAX
        </Text>
      </View>
    </View>
  );
};

export default MainLayout;
