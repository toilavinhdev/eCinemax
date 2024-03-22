import {
  Entypo,
  FontAwesome,
  FontAwesome5,
  FontAwesome6,
} from "@expo/vector-icons";
import { Tabs } from "expo-router";
import React from "react";

const MainLayout = () => {
  return (
    <Tabs initialRouteName="index">
      <Tabs.Screen
        name="index"
        options={{
          title: "Home",
          tabBarIcon: () => <Entypo name="home" size={24} color="black" />,
        }}
      />
      <Tabs.Screen
        name="news"
        options={{
          title: "News",
          tabBarIcon: () => (
            <FontAwesome5 name="newspaper" size={24} color="black" />
          ),
        }}
      />
      <Tabs.Screen
        name="voucher"
        options={{
          title: "Voucher",
          tabBarIcon: () => (
            <FontAwesome6 name="gift" size={22} color="black" />
          ),
        }}
      ></Tabs.Screen>
      <Tabs.Screen
        name="profile"
        options={{
          title: "Profile",
          tabBarIcon: () => <FontAwesome name="user" size={24} color="black" />,
        }}
      ></Tabs.Screen>
    </Tabs>
  );
};

export default MainLayout;
