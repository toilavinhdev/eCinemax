import { View, Text, Alert } from "react-native";
import React, { useState } from "react";
import { ButtonComponent, InputComponent } from "~/shared/components";
import { useAppDispatch, useAppSelector } from "~/features/store";
import { updatePassword } from "~/features/user";

const UpdatePasswordScreen = () => {
  const dispatch = useAppDispatch();
  const loading = useAppSelector((state) => state.user.loadingUpdatePassword);
  const currentUser = useAppSelector((state) => state.user.currentUser);
  const [currentPassword, setCurrentPassword] = useState("");
  const [newPassword, setNewPassword] = useState("");
  const [confirmNewPassword, setConfirmNewPassword] = useState("");

  const onSubmit = () => {
    if (newPassword !== confirmNewPassword) {
      Alert.alert("Mật khẩu nhập lại không khớp");
      return;
    }
    if (!currentUser) return;
    dispatch(
      updatePassword({
        email: currentUser.email,
        newPassword,
        currentPassword,
      })
    );
  };

  return (
    <View className="flex-1 items-center justify-center px-6">
      <Text className="font-medium text-[18px]">Đặt lại mật khẩu của bạn</Text>
      <View className="w-full mt-8">
        <InputComponent
          label="Mật khẩu hiện tại"
          password
          placeholder="Mật khẩu hiện tại"
          value={currentPassword}
          onChangeText={(val) => setCurrentPassword(val)}
          containerClassName="w-full"
        />
        <InputComponent
          label="Mật khẩu mới"
          password
          placeholder="Nhập mật khẩu mới"
          value={newPassword}
          onChangeText={(val) => setNewPassword(val)}
          containerClassName="w-full mt-4"
        />
        <InputComponent
          password
          label="Nhập lại mật khẩu mới"
          placeholder="Xác nhận mật khẩu mới"
          value={confirmNewPassword}
          onChangeText={(val) => setConfirmNewPassword(val)}
          containerClassName="w-full mt-4"
        />
      </View>
      <ButtonComponent
        loading={loading}
        text="Xác nhận"
        onPress={() => onSubmit()}
        buttonClassName="w-full mt-12 h-[55px]"
      />
    </View>
  );
};

export default UpdatePasswordScreen;
