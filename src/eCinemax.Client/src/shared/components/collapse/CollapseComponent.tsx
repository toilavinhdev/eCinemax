import { View, Text, TouchableWithoutFeedback } from "react-native";
import React, { ReactNode, useState } from "react";
import { AntDesign } from "@expo/vector-icons";
import { colors } from "~/shared/constants";
import { IfComponent } from "~/core/components";

interface Props {
  title: string;
  children: ReactNode;
  opend?: boolean;
}

const CollapseComponent = (props: Props) => {
  const { title, children, opend } = props;
  const [opened, setOpened] = useState(opend ?? false);

  const toggle = () => {
    setOpened(!opened);
  };

  return (
    <View className="rounded-lg" style={{ backgroundColor: colors.secondary }}>
      <TouchableWithoutFeedback onPress={() => toggle()}>
        <View className="flex-row justify-between items-center p-5">
          <Text className="text-white text-[16px]">{title}</Text>
          <AntDesign name="right" size={16} color="white" />
        </View>
      </TouchableWithoutFeedback>
      <IfComponent condition={opened}>
        <View className="mt-3">{children}</View>
      </IfComponent>
    </View>
  );
};

export default CollapseComponent;
