import React, { useRef, useState } from "react";
import { Text, TextInput, TouchableWithoutFeedback, View } from "react-native";
import { IfComponent } from "~/core/components";
import { colors } from "~/shared/constants";

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

  const inputRef = useRef<TextInput>(null);
  const [focus, setFocus] = useState(false);

  const containerFocusInput = () => {
    if (inputRef.current) {
      inputRef.current.focus();
    }
  };

  return (
    <TouchableWithoutFeedback onPress={containerFocusInput}>
      <View
        className={`border rounded-md px-4 ${containerClassName}`}
        style={{ borderColor: focus ? "#4b5563" : "#d1d5db" }}
      >
        <IfComponent condition={!!label}>
          <Text className="pt-2 text-[12px]">{label}</Text>
        </IfComponent>
        <TextInput
          ref={inputRef}
          onFocus={() => setFocus(true)}
          onBlur={() => setFocus(false)}
          value={value}
          onChangeText={(value) => onChangeText(value)}
          placeholder={placeholder}
          secureTextEntry={password}
          className={`text-[16px] pt-2 pb-3 ${inputClassName}`}
        />
      </View>
    </TouchableWithoutFeedback>
  );
};

export default InputComponent;
