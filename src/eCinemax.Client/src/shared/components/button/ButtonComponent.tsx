import React from "react";
import { ActivityIndicator, Text, TouchableOpacity } from "react-native";
import { colors } from "~/shared/constants";

interface Props {
  text: string;
  disabled?: boolean;
  loading?: boolean;
  onPress?: () => void;
  buttonClassName?: string;
  textClassName?: string;
  appearance?: "default" | "text";
}

const ButtonComponent = (props: Props) => {
  const {
    text,
    disabled,
    loading,
    onPress,
    buttonClassName,
    textClassName,
    appearance = "default",
  } = props;

  switch (appearance) {
    case "default":
      return (
        <TouchableOpacity
          onPress={onPress}
          disabled={disabled}
          style={{ backgroundColor: !disabled ? colors.primary : colors.gray }}
          className={`flex justify-center items-center w-[150] h-[48] rounded-lg ${buttonClassName}`}
        >
          {!loading ? (
            <Text className={`text-center ${textClassName}`}>{text}</Text>
          ) : (
            <ActivityIndicator />
          )}
        </TouchableOpacity>
      );
    case "text":
      return (
        <TouchableOpacity
          onPress={onPress}
          disabled={disabled}
          className={`flex justify-center items-center w-[150] h-[48] rounded-lg ${buttonClassName}`}
        >
          {!loading ? (
            <Text className={`text-center ${textClassName}`}>{text}</Text>
          ) : (
            <ActivityIndicator />
          )}
        </TouchableOpacity>
      );
  }
};

export default ButtonComponent;