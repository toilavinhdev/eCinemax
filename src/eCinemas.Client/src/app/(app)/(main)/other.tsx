import { MaterialIcons } from "@expo/vector-icons";
import AsyncStorage from "@react-native-async-storage/async-storage";
import { router } from "expo-router";
import React from "react";
import {
  Alert,
  FlatList,
  Image,
  Text,
  TouchableOpacity,
  View,
} from "react-native";
import { useAppDispatch, useAppSelector } from "~/features/store";
import { signOut } from "~/features/user/user.slice";
import { authConst, colors } from "~/shared/constants";

const OtherScreen = () => {
  const user = useAppSelector((state) => state.user.currentUser);
  const dispatch = useAppDispatch();

  const onLogout = async () => {
    const logout = async () => {
      dispatch(signOut());
      router.replace("/auth/sign-in");
      await AsyncStorage.removeItem(authConst.ACCESS_TOKEN);
    };

    Alert.alert("Bạn có muốn đăng xuất không", undefined, [
      {
        text: "Đồng ý",
        onPress: () => logout(),
      },
      {
        text: "Hủy",
        onPress: () => {},
        style: "cancel",
      },
    ]);
  };

  return (
    <View style={{ backgroundColor: colors.dark, flex: 1 }} className="px-4">
      {user && (
        <View className="flex-row items-center gap-x-6 mt-6">
          <Image
            source={require("../../../shared/assets/images/default-avatar.jpg")}
            className="h-[54px] w-[54px] rounded-full"
          />
          <Text className="text-white text-[18px] font-medium">
            {user?.fullName}
          </Text>
        </View>
      )}

      <FlatList
        data={[
          {
            title: "Ngôn ngữ",
            icon: <MaterialIcons name="language" size={24} color="white" />,
          },
          {
            title: "Đổi mật khẩu",
            icon: <MaterialIcons name="password" size={24} color="white" />,
            onPress: () => router.push("/auth/update-password"),
          },
          {
            title: "Lịch sử giao dịch",
            icon: <MaterialIcons name="history" size={28} color="white" />,
          },
          {
            title: "Đăng xuất",
            icon: <MaterialIcons name="logout" size={24} color={"white"} />,
            onPress: () => onLogout(),
          },
        ]}
        renderItem={({ item }) => (
          <TouchableOpacity
            onPress={item.onPress}
            className="flex-row items-center gap-x-3 border-b border-gray-700 py-4"
          >
            <View className="w-[30px] items-center">{item.icon}</View>
            <Text className={`text-white`}>{item.title}</Text>
          </TouchableOpacity>
        )}
        className="mt-2"
      />
    </View>
  );
};

export default OtherScreen;
