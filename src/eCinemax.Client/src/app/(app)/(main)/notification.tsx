import { View, Text } from "react-native";
import React from "react";
import { colors } from "~/shared/constants";
import { NoDataComponent } from "~/shared/components";

const NotificationScreen = () => {
  return (
    <View className="flex-1" style={{ backgroundColor: colors.dark }}>
      <NoDataComponent />
    </View>
  );
};

export default NotificationScreen;
