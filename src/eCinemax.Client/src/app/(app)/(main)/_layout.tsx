import {
  Entypo,
  FontAwesome,
  MaterialCommunityIcons,
  MaterialIcons,
} from "@expo/vector-icons";
import { Redirect, Tabs } from "expo-router";
import React from "react";
import { Text, View } from "react-native";
import { useSafeAreaInsets } from "react-native-safe-area-context";
import { useAppSelector } from "~/features/store";
import { colors } from "~/shared/constants";

const MainLayout = () => {
  const { authenticated } = useAppSelector((state) => state.user);

  if (!authenticated) {
    return <Redirect href="/auth/sign-in" />;
  }

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
          ECINEMAX
        </Text>
      </View>
    </View>
  );
};

export default MainLayout;
