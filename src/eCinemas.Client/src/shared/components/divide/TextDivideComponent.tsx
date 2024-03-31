import { View, Text } from "react-native";
import React from "react";

interface Props {
  text: string;
  textClassName?: string;
  containerClassName?: string;
}

const TextDivideComponent = (props: Props) => {
  const { text, textClassName, containerClassName } = props;
  return (
    <View className={`flex-row items-center ${containerClassName}`}>
      <View className="flex-1 h-px bg-gray-400"></View>
      <Text className={`mx-3 ${textClassName}`}>{text}</Text>
      <View className="flex-1 h-px bg-gray-400"></View>
    </View>
  );
};

export default TextDivideComponent;
