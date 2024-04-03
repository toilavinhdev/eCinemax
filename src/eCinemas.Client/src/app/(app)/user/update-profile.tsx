import {
  View,
  Text,
  KeyboardAvoidingView,
  Platform,
  Alert,
} from "react-native";
import React, { useEffect, useState } from "react";
import { ButtonComponent, InputComponent } from "~/shared/components";
import { router } from "expo-router";
import { useAppDispatch, useAppSelector } from "~/features/store";
import { isEmailValid } from "~/shared/utils";
import { updateProfile } from "~/features/user";

const UpdateProfileScreen = () => {
  const [fullName, setFullName] = useState<string>("");
  const [email, setEmail] = useState<string>("");
  const user = useAppSelector((state) => state.user.currentUser);
  const status = useAppSelector((state) => state.user.status);
  const dispatch = useAppDispatch();

  const onSubmit = () => {
    if (!isEmailValid(email)) Alert.alert("Email không đúng định dạng");
    if (!fullName || !email) return;
    dispatch(updateProfile({ fullName, email }));
  };

  useEffect(() => {
    if (user) {
      setFullName(user.fullName);
      setEmail(user.email);
    }
  }, []);

  return (
    <KeyboardAvoidingView
      behavior={Platform.OS === "ios" ? "padding" : "height"}
      className="flex-1 px-6"
    >
      <View className="w-full mt-8">
        <InputComponent
          label="Tên của bạn"
          value={fullName}
          onChangeText={(val) => setFullName(val)}
          containerClassName="w-full"
        />
        <InputComponent
          label="Email"
          value={email}
          onChangeText={(val) => setEmail(val)}
          containerClassName="w-full mt-4"
        />
      </View>
      <View className="mt-auto mb-[40px]">
        <ButtonComponent
          loading={status === "loading"}
          disabled={status === "loading" || !fullName || !email}
          onPress={onSubmit}
          text="Cập nhật"
          textClassName="font-semibold text-[18px]"
          buttonClassName="mt-8 w-full h-[60px]"
        />
        <ButtonComponent
          disabled={status === "loading"}
          text="Hủy"
          onPress={() => router.back()}
          textClassName="font-semibold text-[18px]"
          buttonClassName="mt-2 w-full h-[60px]"
          appearance="text"
        />
      </View>
    </KeyboardAvoidingView>
  );
};

export default UpdateProfileScreen;
