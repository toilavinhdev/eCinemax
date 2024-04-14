import { View, Text, ActivityIndicator } from "react-native";
import React from "react";
import { colors } from "~/shared/constants";

const SpinnerFooterComponent = (props: { show: boolean }) => {
  const { show } = props;

  return (
    <>
      {show && (
        <View className="flex-row items-center justify-center gap-3 my-3">
          <ActivityIndicator color={colors.primary} />
          <Text className="text-white">Đang tải...</Text>
        </View>
      )}
    </>
  );
};

export default SpinnerFooterComponent;
