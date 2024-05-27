import React, { ReactNode } from "react";
import { ActivityIndicator, Text, TouchableOpacity } from "react-native";
import { IfComponent } from "~/core/components";
import { colors } from "~/shared/constants";

interface Props {
  text?: string;
  content?: ReactNode;
  disabled?: boolean;
  loading?: boolean;
  onPress?: () => void;
  buttonClassName?: string;
  textClassName?: string;
  appearance?: "default" | "text";
  bgColor?: string;
}

const ButtonComponent = (props: Props) => {
  const {
    text,
    content,
    disabled,
    loading,
    onPress,
    buttonClassName,
    textClassName,
    appearance = "default",
    bgColor,
  } = props;

  switch (appearance) {
    case "default":
      return (
        <TouchableOpacity
          onPress={onPress}
          disabled={disabled}
          style={{
            backgroundColor: !disabled
              ? bgColor ?? colors.primary
              : colors.gray,
          }}
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
          <IfComponent
            condition={!loading}
            elseTemplate={<ActivityIndicator />}
          >
            <IfComponent condition={!content} elseTemplate={content}>
              <Text className={`text-center ${textClassName}`}>{text}</Text>
            </IfComponent>
          </IfComponent>
        </TouchableOpacity>
      );
  }
};

export default ButtonComponent;
