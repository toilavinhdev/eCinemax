import { router } from "expo-router";
import React, { useEffect, useState } from "react";
import { Alert, ScrollView, Text } from "react-native";
import { useAppDispatch, useAppSelector } from "~/features/store";
import { refreshStatus, signUp } from "~/features/user";
import { ButtonComponent, InputComponent } from "~/shared/components";
import { isEmailValid, isEmptyOrWhitespace } from "~/shared/utils";

const SignUpScreen = () => {
  const [fullName, setFullName] = useState<string>("");
  const [email, setEmail] = useState<string>("");
  const [password, setPassword] = useState<string>("");
  const [confirmPassword, setConfirmPassword] = useState<string>("");
  const dispatch = useAppDispatch();
  const { status, error } = useAppSelector((state) => state.user);

  const onSubmit = () => {
    if (isEmptyOrWhitespace(fullName)) {
      Alert.alert("Vui lòng nhập tên");
      return;
    }
    if (isEmptyOrWhitespace(email)) {
      Alert.alert("Vui lòng nhập email");
      return;
    }
    if (isEmptyOrWhitespace(password) || isEmptyOrWhitespace(confirmPassword)) {
      Alert.alert("Vui lòng nhập mật khẩu");
      return;
    }
    if (!isEmailValid(email)) {
      Alert.alert("Email không đúng định dạng");
      return;
    }
    if (password !== confirmPassword) {
      Alert.alert("Mật khẩu nhập lại không khớp");
      return;
    }

    dispatch(signUp({ email, fullName, password }));
  };

  useEffect(() => {
    if (status === "failed" && error) {
      Alert.alert(error, undefined, [
        {
          onPress: () => dispatch(refreshStatus()),
        },
      ]);
    } else if (status === "success") {
      Alert.alert("Đăng ký thành công", undefined, [
        {
          onPress: () => {
            dispatch(refreshStatus());
            router.replace("/auth/sign-in");
          },
        },
      ]);
    }
  }, [status]);

  return (
    <ScrollView className="flex-1 bg-white px-8">
      <Text className="font-medium text-[36px] mt-20">Sign Up {status}</Text>
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
        disabled={status === "loading"}
        onPress={() => onSubmit()}
        textClassName="font-semibold text-[18px]"
        buttonClassName="mt-8 w-full h-[60px]"
      />
      <ButtonComponent
        text="Đã có tài khoản? Đăng nhập ngay"
        disabled={status === "loading"}
        onPress={() => router.push("/auth/sign-in")}
        textClassName="font-semibold text-[14px]"
        buttonClassName="w-full mt-3 mb-10"
        appearance="text"
      />
    </ScrollView>
  );
};

export default SignUpScreen;
