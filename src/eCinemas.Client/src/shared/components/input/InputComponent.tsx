import React from "react";
import { TextInput, View } from "react-native";

interface Props {
  value: string;
  onChangeText: (val: string) => void;
  placeholder?: string;
  password?: boolean;
  containerClassName?: string;
  inputClassName?: string;
}

const InputComponent = (props: Props) => {
  const {
    value,
    onChangeText,
    placeholder,
    password,
    containerClassName,
    inputClassName,
  } = props;

  return (
    <View
      className={`border border-gray-300 rounded-md px-4 ${containerClassName}`}
    >
      <TextInput
        value={value}
        onChangeText={(value) => onChangeText(value)}
        placeholder={placeholder}
        secureTextEntry={password}
        className={`text-[16px] py-5 ${inputClassName}`}
      />
    </View>
  );
};

export default InputComponent;
