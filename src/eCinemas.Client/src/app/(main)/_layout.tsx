import { View, Text } from "react-native";
import React from "react";
import { I18nManager } from "../../localization/i18n";

export default function MainLayout() {
  const i18n = I18nManager("vi");

  return (
    <View className="flex-1 justify-center items-center">
      <Text>{i18n.t("HelloWorld")}</Text>
    </View>
  );
}
