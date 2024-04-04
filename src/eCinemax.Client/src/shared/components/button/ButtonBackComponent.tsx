import React from "react";
import { Text, TouchableOpacity } from "react-native";

interface Props {
  text: string;
  onPress?: () => void;
}

const ButtonBackComponent = (props: Props) => {
  const { text, onPress } = props;

  return (
    <TouchableOpacity onPress={onPress}>
      <Text className="text-white text-[16px]">{text}</Text>
    </TouchableOpacity>
  );
};

export default ButtonBackComponent;
