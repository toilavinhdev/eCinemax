import { Link } from "expo-router";
import React, { useState } from "react";
import { Text, View } from "react-native";
import { ButtonComponent, InputComponent } from "~/shared/components";
import { isEmailValid } from "~/shared/utils";
import { useAppDispatch, useAppSelector } from "~/features/store";
import { signIn } from "~/features/user";

const SignInScreen = () => {
  const [email, setEmail] = useState("hoangdvinh68@gmail.com");
  const [password, setPassword] = useState("Password@123");
  const dispatch = useAppDispatch();
  const loading = useAppSelector((state) => state.user.loading);

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
        value={email}
        onChangeText={(val) => setEmail(val)}
        placeholder="enter your email"
        containerClassName="mt-9"
      />
      <InputComponent
        value={password}
        password={true}
        onChangeText={(val) => setPassword(val)}
        placeholder="enter your password"
        containerClassName="mt-4"
      />
      <ButtonComponent
        text="Login"
        loading={loading}
        disabled={!isEmailValid(email) || !password || loading}
        onPress={onSubmit}
        textClassName="font-semibold text-[18px]"
        buttonClassName="mt-8 w-full h-[60px]"
      />
      <Text className="text-center mt-auto mb-10 text-[15px]">
        Do not have an account?{" "}
        <Link href="/auth/sign-up" className="underline">
          Sign up
        </Link>
      </Text>
    </View>
  );
};

export default SignInScreen;
