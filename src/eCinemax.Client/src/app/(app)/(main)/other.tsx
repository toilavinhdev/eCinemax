import { FontAwesome, FontAwesome5, MaterialIcons } from "@expo/vector-icons";
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
import { IfComponent } from "~/core/components";
import { useAppSelector } from "~/features/store";
import { authConst, colors } from "~/shared/constants";

const OtherScreen = () => {
  const user = useAppSelector((state) => state.user.currentUser);

  const onLogout = async () => {
    Alert.alert("Bạn có muốn đăng xuất không", undefined, [
      {
        text: "Đồng ý",
        onPress: async () => {
          await AsyncStorage.removeItem(authConst.ACCESS_TOKEN);
          router.replace("/auth/sign-in");
        },
        style: "destructive",
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
        <View className="flex-row items-center justify-between gap-x-3 mt-6">
          <View className="flex-row items-center gap-x-3">
            <IfComponent
              condition={!!user.avatarUrl}
              elseTemplate={
                <Image
                  source={require("../../../shared/assets/images/default-avatar.jpg")}
                  className="h-[54px] w-[54px] rounded-full"
                />
              }
            >
              <Image
                source={{ uri: user.avatarUrl }}
                className="h-[54px] w-[54px] rounded-full"
              />
            </IfComponent>

            <View className="space-y-1">
              <Text className="text-white text-[18px] font-medium">
                {user?.fullName}
              </Text>
              <Text className="text-[12px] text-gray-300">{user?.email}</Text>
            </View>
          </View>
          <TouchableOpacity onPress={() => router.push("/user/update-profile")}>
            <Text>
              <FontAwesome5 name="user-edit" size={24} color="white" />,
            </Text>
          </TouchableOpacity>
        </View>
      )}

      <FlatList
        data={[
          {
            title: "Bộ sưu tập",
            icon: <FontAwesome name="bookmark" size={22} color="white" />,
            onPress: () => {
              router.push("/collection");
            },
          },
          {
            title: "Đổi mật khẩu",
            icon: <MaterialIcons name="password" size={24} color="white" />,
            onPress: () => router.push("/user/update-password"),
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
        showsVerticalScrollIndicator={false}
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
