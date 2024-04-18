import React, { useRef, useState } from "react";
import {
  Text,
  TextInput,
  TouchableOpacity,
  TouchableWithoutFeedback,
  View,
  ViewBase,
} from "react-native";
import { IfComponent } from "~/core/components";
import { Ionicons } from "@expo/vector-icons";
import { colors } from "~/shared/constants";

interface Props {
  value: string;
  onChangeText: (val: string) => void;
  placeholder?: string;
  label?: string;
  password?: boolean;
  maxLength?: number;
  containerClassName?: string;
  inputClassName?: string;
  labelClassName?: string;
}

const InputComponent = (props: Props) => {
  const {
    value,
    onChangeText,
    placeholder,
    label,
    password,
    maxLength,
    containerClassName,
    inputClassName,
    labelClassName,
  } = props;

  const inputRef = useRef<TextInput>(null);
  const [focus, setFocus] = useState(false);
  const [showPassword, setShowPassword] = useState(false);

  const containerFocusInput = () => {
    if (inputRef.current) {
      inputRef.current.focus();
    }
  };

  return (
    <TouchableWithoutFeedback onPress={containerFocusInput}>
      <View
        className={`flex-row items-center border rounded-md px-4 ${containerClassName}`}
        style={{
          borderColor: focus ? "#4b5563" : "#d1d5db",
        }}
      >
        <View className="flex-1">
          <IfComponent condition={!!label}>
            <Text className={`pt-2 text-[12px] ${labelClassName}`}>
              {label}
            </Text>
          </IfComponent>
          <TextInput
            ref={inputRef}
            onFocus={() => setFocus(true)}
            onBlur={() => setFocus(false)}
            value={value}
            onChangeText={(value) => onChangeText(value)}
            placeholder={placeholder}
            secureTextEntry={password && !showPassword}
            maxLength={maxLength}
            className={`text-[16px] pt-2 pb-3 ${inputClassName}`}
          />
        </View>

        {password && (
          <TouchableOpacity
            className="ml-3"
            onPress={() => setShowPassword(!showPassword)}
          >
            <Ionicons
              name={showPassword ? "eye" : "eye-off"}
              size={25}
              color={focus ? "#4b5563" : "#d1d5db"}
            />
          </TouchableOpacity>
        )}

        {maxLength && (
          <View>
            <Text className="text-white">
              {value.length}/{maxLength}
            </Text>
          </View>
        )}
      </View>
    </TouchableWithoutFeedback>
  );
};

export default InputComponent;
