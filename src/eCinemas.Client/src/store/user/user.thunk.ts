import { createAsyncThunk } from "@reduxjs/toolkit";
import {
  ISignInRequest,
  ISignUpRequest,
  IUserClaimValue,
} from "~/store/user/user.interfaces";
import { getMeAPI, signInAPI, signUpAPI } from "~/store/user/user.apis";
import AsyncStorage from "@react-native-async-storage/async-storage";
import { authConst } from "~/shared/constants";
import { redirectToAuthScreen, redirectToMainScreen } from "~/shared/utils";
import { jwtDecode } from "jwt-decode";
import { useRouter } from "expo-router";

export const signIn = createAsyncThunk(
  "user/signIn",
  async (payload: ISignInRequest, thunkAPI) => {
    try {
      const response = await signInAPI(payload);
      const accessToken = response.data.data.accessToken;
      const userClaims = jwtDecode<IUserClaimValue>(accessToken);
      await AsyncStorage.setItem(authConst.ACCESS_TOKEN, accessToken);
      await AsyncStorage.setItem(
        authConst.USER_DATA,
        JSON.stringify(userClaims)
      );
      redirectToMainScreen();
    } catch (err) {}
  }
);

export const signUp = createAsyncThunk(
  "user/signUp",
  async (payload: ISignUpRequest, thunkAPI) => {
    try {
      await signUpAPI(payload);
    } catch (err) {}
  }
);

export const signOut = createAsyncThunk("user/signOut", async () => {
  const router = useRouter();
  await AsyncStorage.removeItem(authConst.ACCESS_TOKEN);
  await AsyncStorage.removeItem(authConst.USER_DATA);
  router.replace("/");
});

export const getMe = createAsyncThunk("user/me", async () => {
  try {
    const response = await getMeAPI();
    return response.data.data;
  } catch (err) {
    console.log(err);
  }
});
