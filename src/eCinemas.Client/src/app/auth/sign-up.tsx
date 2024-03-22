import { Link } from "expo-router";
import React, { useState } from "react";
import { Text, View } from "react-native";
import { ButtonComponent, InputComponent } from "~/shared/components";

const SignUpScreen = () => {
  const [fullName, setFullName] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");

  return (
    <View className="flex-1 bg-white px-8">
      <Text className="font-medium text-[36px] mt-20">Sign Up</Text>
      <Text className="font-light text-[12px] mt-2">
        Sign up with on of following options
      </Text>
      <InputComponent
        value={fullName}
        onChangeText={(val) => setFullName(val)}
        placeholder="enter your full name"
        containerClassName="mt-10"
      />
      <InputComponent
        value={email}
        onChangeText={(val) => setEmail(val)}
        placeholder="enter your email"
        containerClassName="mt-4"
      />
      <InputComponent
        value={password}
        onChangeText={(val) => setPassword(val)}
        placeholder="enter your password"
        containerClassName="mt-4"
      />
      <ButtonComponent
        text="Sign up"
        textClassName="font-semibold text-[18px]"
        buttonClassName="mt-8 w-full h-[60px]"
      />
      <Text className="text-center mt-auto mb-10 text-[15px]">
        Already have an account?{" "}
        <Link href="/auth/sign-in" className="underline">
          Login
        </Link>
      </Text>
    </View>
  );
};

export default SignUpScreen;
