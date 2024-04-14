import React from "react";
import { ActivityIndicator, View } from "react-native";
import { useAppSelector } from "~/features/store";
import { colors } from "~/shared/constants";

const SpinnerOverlayComponent = () => {
  const { globalLoading } = useAppSelector((state) => state.common);

  if (!globalLoading) return null;

  return (
    <View className="absolute bottom-0 h-screen w-screen flex items-center justify-center">
      <View className="bg-gray-300 bg-opacity-5 h-[160px] w-[160px] flex items-center justify-center rounded-2xl">
        <ActivityIndicator size="large" color={colors.dark} />
      </View>
    </View>
  );
};

export default SpinnerOverlayComponent;
