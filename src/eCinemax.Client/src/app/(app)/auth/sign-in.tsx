import { router } from "expo-router";
import React, { useEffect, useState } from "react";
import { Alert, KeyboardAvoidingView, Platform, Text } from "react-native";
import { useAppDispatch, useAppSelector } from "~/features/store";
import { refreshStatus, signIn } from "~/features/user";
import {
  ButtonComponent,
  InputComponent,
  TextDivideComponent,
} from "~/shared/components";
import { isEmailValid, isEmptyOrWhitespace } from "~/shared/utils";

const SignInScreen = () => {
  const [email, setEmail] = useState<string>("");
  const [password, setPassword] = useState<string>("");
  const dispatch = useAppDispatch();
  const { status, error } = useAppSelector((state) => state.user);

  const onSubmit = async () => {
    if (isEmptyOrWhitespace(email)) {
      Alert.alert("Vui lòng nhập email");
      return;
    }
    if (isEmptyOrWhitespace(password)) {
      Alert.alert("Vui lòng nhập mật khẩu");
      return;
    }
    if (!isEmailValid(email)) {
      Alert.alert("Email không đúng định dạng");
      return;
    }
    if (password.length < 6) {
      Alert.alert("Mật khẩu tối thiểu 6 ký tự");
      return;
    }
    dispatch(signIn({ email, password }));
  };

  useEffect(() => {
    if (status === "failed" && error) {
      Alert.alert(error);
      dispatch(refreshStatus());
      return;
    }
    if (status === "success") {
      dispatch(refreshStatus());
      return;
    }
  }, [status]);

  return (
    <KeyboardAvoidingView
      behavior={Platform.OS === "ios" ? "padding" : "height"}
      className="flex-1 bg-white px-8"
    >
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
        disabled={status === "loading"}
        onPress={onSubmit}
        textClassName="font-semibold text-[18px]"
        buttonClassName="mt-8 w-full h-[60px]"
      />
      <TextDivideComponent text="Hoặc" containerClassName="my-10" />
      <ButtonComponent
        text="Chưa có tài khoản? Tạo tài khoản mới"
        onPress={() => router.push("/auth/sign-up")}
        disabled={status === "loading"}
        textClassName="font-semibold text-[14px]"
        buttonClassName="w-full mt-auto mb-10"
        appearance="text"
      />
    </KeyboardAvoidingView>
  );
};

export default SignInScreen;
