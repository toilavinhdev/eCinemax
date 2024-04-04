import React, { useState } from "react";
import { Alert, Text, View } from "react-native";
import { useAppDispatch, useAppSelector } from "~/features/store";
import { signUp } from "~/features/user";
import { ButtonComponent, InputComponent } from "~/shared/components";
import { isEmailValid } from "~/shared/utils";
import { router } from "expo-router";

const SignUpScreen = () => {
  const [fullName, setFullName] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("Password@123");
  const [confirmPassword, setConfirmPassword] = useState("Password@123");
  const dispatch = useAppDispatch();
  const status = useAppSelector((state) => state.user.status);

  const onSubmit = () => {
    if (password !== confirmPassword) {
      Alert.alert("Mật khẩu nhập lại không khớp");
      return;
    }

    dispatch(signUp({ email, fullName, password }));
  };

  return (
    <View className="flex-1 bg-white px-8">
      <Text className="font-medium text-[36px] mt-20">Sign Up</Text>
      <Text className="font-light text-[12px] mt-2">
        Sign up with on of following options
      </Text>
      <InputComponent
        label="Tên đầy đủ"
        value={fullName}
        onChangeText={(val) => setFullName(val)}
        placeholder="enter your full name"
        containerClassName="mt-10"
      />
      <InputComponent
        label="Địa chỉ email"
        value={email}
        onChangeText={(val) => setEmail(val)}
        placeholder="enter your email"
        containerClassName="mt-4"
      />
      <InputComponent
        label="Mật khẩu"
        password
        value={password}
        onChangeText={(val) => setPassword(val)}
        placeholder="enter your password"
        containerClassName="mt-4"
      />
      <InputComponent
        password
        label="Xác nhận mật khẩu"
        value={confirmPassword}
        onChangeText={(val) => setConfirmPassword(val)}
        placeholder="enter your confirm password"
        containerClassName="mt-4"
      />
      <ButtonComponent
        text="Đăng ký"
        loading={status === "loading"}
        disabled={!fullName || !password || !isEmailValid(email)}
        onPress={() => onSubmit()}
        textClassName="font-semibold text-[18px]"
        buttonClassName="mt-8 w-full h-[60px]"
      />
      <ButtonComponent
        text="Đã có tài khoản? Đăng nhập ngay"
        onPress={() => router.push("/auth/sign-in")}
        textClassName="font-semibold text-[14px]"
        buttonClassName="w-full mt-auto mb-10"
        appearance="text"
      />
    </View>
  );
};

export default SignUpScreen;
