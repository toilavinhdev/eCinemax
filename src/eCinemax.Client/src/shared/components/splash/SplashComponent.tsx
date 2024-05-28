import { View, Text, ActivityIndicator } from "react-native";
import React from "react";
import { colors } from "~/shared/constants";

const SplashComponent = () => {
  return (
    <View
      className="flex-1 items-center justify-center"
      style={{ backgroundColor: colors.dark }}
    >
      <Text
        className="font-[900] text-[60px]"
        style={{ color: colors.primary }}
      >
        CINEMAX
      </Text>
      <ActivityIndicator className="mt-8" color={colors.primary} />
    </View>
  );
};

export default SplashComponent;
