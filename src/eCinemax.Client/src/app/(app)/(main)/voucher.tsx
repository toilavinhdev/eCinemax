import React from "react";
import { View } from "react-native";
import { NoDataComponent } from "~/shared/components";
import { colors } from "~/shared/constants";

const VoucherScreen = () => {
  return (
    <View className="flex-1" style={{ backgroundColor: colors.dark }}>
      <NoDataComponent />
    </View>
  );
};

export default VoucherScreen;
