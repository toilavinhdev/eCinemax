import { View, Text } from "react-native";
import React from "react";
import { useAppDispatch, useAppSelector } from "~/store/store";
import { ButtonComponent } from "~/shared/components";
import { signOut } from "~/store/user/user.thunk";

const ProfileScreen = () => {
  const user = useAppSelector((state) => state.user.currentUser);
  const dispatch = useAppDispatch();

  return (
    <View>
      <Text>fullName: '{user?.fullName}'</Text>
      <ButtonComponent text="Logout" onPress={() => dispatch(signOut())} />
    </View>
  );
};

export default ProfileScreen;
