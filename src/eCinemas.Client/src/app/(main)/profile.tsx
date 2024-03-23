import React from "react";
import { Text, View } from "react-native";
import { useAppDispatch, useAppSelector } from "~/features/store";
import { signOut } from "~/features/user/user.slice";
import { ButtonComponent } from "~/shared/components";

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
