import React from "react";
import { TextInput, Text, View } from "react-native";
import { IfComponent } from "~/core/components";

interface Props {
  value: string;
  onChangeText: (val: string) => void;
  placeholder?: string;
  label?: string;
  password?: boolean;
  containerClassName?: string;
  inputClassName?: string;
}

const InputComponent = (props: Props) => {
  const {
    value,
    onChangeText,
    placeholder,
    label,
    password,
    containerClassName,
    inputClassName,
  } = props;

  return (
    <View
      className={`border border-gray-300 rounded-md px-4 ${containerClassName}`}
    >
      <IfComponent condition={!!label}>
        <Text className="pt-2 text-[12px]">{label}</Text>
      </IfComponent>
      <TextInput
        value={value}
        onChangeText={(value) => onChangeText(value)}
        placeholder={placeholder}
        secureTextEntry={password}
        className={`text-[16px] py-2 ${inputClassName}`}
      />
    </View>
  );
};

export default InputComponent;
