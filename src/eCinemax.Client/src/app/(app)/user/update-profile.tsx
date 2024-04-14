import { router } from "expo-router";
import React, { useEffect, useState } from "react";
import { Alert, ScrollView, View } from "react-native";
import { useAppDispatch, useAppSelector } from "~/features/store";
import { refreshStatus, updateProfile } from "~/features/user";
import { ButtonComponent, InputComponent } from "~/shared/components";
import { isEmailValid, isEmptyOrWhitespace } from "~/shared/utils";

const UpdateProfileScreen = () => {
  const [fullName, setFullName] = useState<string>("");
  const [email, setEmail] = useState<string>("");
  const { status, error, currentUser } = useAppSelector((state) => state.user);
  const dispatch = useAppDispatch();

  const fillData = () => {
    if (currentUser) {
      setFullName(currentUser.fullName);
      setEmail(currentUser.email);
    }
  };

  const onSubmit = () => {
    if (isEmptyOrWhitespace(fullName)) {
      Alert.alert("Vui lòng nhập tên");
      return;
    }
    if (isEmptyOrWhitespace(email)) {
      Alert.alert("Vui lòng nhập email");
      return;
    }
    if (!isEmailValid(email)) Alert.alert("Email không đúng định dạng");
    dispatch(updateProfile({ fullName, email }));
  };

  useEffect(() => {
    fillData();
  }, []);

  useEffect(() => {
    if (status === "failed" && error) {
      Alert.alert(error);
      dispatch(refreshStatus());
      return;
    }
    if (status === "success") {
      Alert.alert("Cập nhật thông tin thành công", undefined, [
        {
          onPress: () => {
            router.replace("/other");
            dispatch(refreshStatus());
          },
        },
      ]);
    }
  }, [status]);

  return (
    <ScrollView className="flex-1 px-6">
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
          disabled={status === "loading"}
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
    </ScrollView>
  );
};

export default UpdateProfileScreen;
