import { router } from "expo-router";
import React, { useEffect, useState } from "react";
import { Alert, Text, View } from "react-native";
import {
  ButtonComponent,
  InputComponent,
  TextDivideComponent,
} from "~/shared/components";
import { isEmailValid } from "~/shared/utils";
import { useAppDispatch, useAppSelector } from "~/features/store";
import { signIn } from "~/features/user";

const SignInScreen = () => {
  const [email, setEmail] = useState("hoangdvinh68@gmail.com");
  const [password, setPassword] = useState("Password@123");
  const dispatch = useAppDispatch();
  const status = useAppSelector((state) => state.user.status);

  const onSubmit = async () => {
    if (!isEmailValid(email) || !password) return;
    dispatch(signIn({ email, password }));
  };

  return (
    <View className="flex-1 bg-white px-8">
      <Text className="font-medium text-[36px] mt-20">Login</Text>
      <Text className="font-light text-[12px] mt-2">
        Login with on of following options
      </Text>
      <InputComponent
        label="Địa chỉ email"
        value={email}
        onChangeText={(val) => setEmail(val)}
        placeholder="Nhập địa chỉ email"
        containerClassName="mt-9"
      />
      <InputComponent
        label="Mật khẩu"
        password
        value={password}
        onChangeText={(val) => setPassword(val)}
        placeholder="Nhập mật khẩu"
        containerClassName="mt-4"
      />
      <ButtonComponent
        text="Đăng nhập"
        loading={status === "loading"}
        disabled={!isEmailValid(email) || !password || status === "loading"}
        onPress={onSubmit}
        textClassName="font-semibold text-[18px]"
        buttonClassName="mt-8 w-full h-[60px]"
      />
      <TextDivideComponent text="Hoặc" containerClassName="my-10" />
      <ButtonComponent
        text="Chưa có tài khoản? Tạo tài khoản mới"
        onPress={() => router.push("/auth/sign-up")}
        textClassName="font-semibold text-[14px]"
        buttonClassName="w-full mt-auto mb-10"
        appearance="text"
      />
    </View>
  );
};

export default SignInScreen;