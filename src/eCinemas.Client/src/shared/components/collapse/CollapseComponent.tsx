import { View, Text, TouchableWithoutFeedback } from "react-native";
import React, { ReactNode, useState } from "react";
import { AntDesign } from "@expo/vector-icons";
import { colors } from "~/shared/constants";

interface Props {
  title: string;
  children: ReactNode;
}

const CollapseComponent = (props: Props) => {
  const { title, children } = props;
  const [opened, setOpened] = useState(false);

  const toggle = () => {
    setOpened(!opened);
  };

  return (
    <View className="rounded-lg" style={{ backgroundColor: colors.secondary }}>
      <TouchableWithoutFeedback onPress={() => toggle()}>
        <View className="flex-row justify-between p-5">
          <Text className="text-white text-[16px]">{title}</Text>
          <AntDesign name="right" size={16} color="white" />
        </View>
      </TouchableWithoutFeedback>
      {opened && <View className="mt-3">{children}</View>}
    </View>
  );
};

export default CollapseComponent;
