import { View, Text } from "react-native";
import React from "react";
import { InputComponent } from "~/shared/components";

const UpdateProfileScreen = () => {
  return (
    <View className="flex-1 items-center justify-center px-6">
      <Text className="font-medium text-[18px]">Hồ sơ của bạn</Text>
      <View className="w-full">
        <InputComponent
          label="Tên của bạn"
          value={""}
          onChangeText={() => {}}
          containerClassName="w-full"
        />
        <InputComponent
          label="Email"
          value={""}
          onChangeText={() => {}}
          containerClassName="w-full mt-3"
        />
      </View>
    </View>
  );
};

export default UpdateProfileScreen;
