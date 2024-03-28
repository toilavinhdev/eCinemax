import { MaterialIcons } from "@expo/vector-icons";
import AsyncStorage from "@react-native-async-storage/async-storage";
import { router } from "expo-router";
import React from "react";
import { FlatList, Image, Text, TouchableOpacity, View } from "react-native";
import { useSafeAreaInsets } from "react-native-safe-area-context";
import { useAppDispatch, useAppSelector } from "~/features/store";
import { signOut } from "~/features/user/user.slice";
import { authConst, colors } from "~/shared/constants";

const ProfileScreen = () => {
  const insets = useSafeAreaInsets();
  const user = useAppSelector((state) => state.user.currentUser);
  const dispatch = useAppDispatch();

  const onLogout = async () => {
    dispatch(signOut());
    router.replace("/auth");
    await AsyncStorage.removeItem(authConst.ACCESS_TOKEN);
  };

  return (
    <View
      style={{ paddingTop: insets.top, backgroundColor: colors.dark, flex: 1 }}
      className="px-4"
    >
      <View className="flex-row items-center gap-x-6 mt-4">
        <Image
          source={require("../../../shared/assets/images/default-avatar.jpg")}
          className="h-[54px] w-[54px] rounded-full"
        />
        <Text className="text-white text-[18px] font-medium">
          {user?.fullName}
        </Text>
      </View>

      <FlatList
        data={[
          {
            title: "Transaction history",
            icon: <MaterialIcons name="history" size={28} color="white" />,
            onPress: () => {
              router.push("/profile/transaction-history");
            },
          },
          {
            title: "Logout",
            icon: <MaterialIcons name="logout" size={24} color={"white"} />,
            onPress: () => onLogout(),
            textClassName: "text-red-400",
          },
        ]}
        renderItem={({ item }) => (
          <TouchableOpacity
            onPress={item.onPress}
            className="flex-row items-center gap-x-3 border-b border-gray-700 py-4"
          >
            <View className="w-[30px] items-center">{item.icon}</View>
            <Text className={`text-white font-semibold ${item.textClassName}`}>
              {item.title}
            </Text>
          </TouchableOpacity>
        )}
        className="mt-2"
      />
    </View>
  );
};

export default ProfileScreen;
