import AsyncStorage from "@react-native-async-storage/async-storage";
import { createAsyncThunk } from "@reduxjs/toolkit";
import { router } from "expo-router";
import { getMeAPI, signInAPI, signUpAPI } from "~/features/user/user.apis";
import {
  ISignInRequest,
  ISignUpRequest,
} from "~/features/user/user.interfaces";
import { authConst } from "~/shared/constants";

export const signIn = createAsyncThunk(
  "@user/signIn",
  async (payload: ISignInRequest, thunkAPI) => {
    try {
      const response = await signInAPI(payload);
      const accessToken = response.data.data.accessToken;
      await AsyncStorage.setItem(authConst.ACCESS_TOKEN, accessToken);
      router.replace("/");
      return accessToken;
    } catch (err: any) {
      thunkAPI.rejectWithValue(err);
    }
  }
);

export const signUp = createAsyncThunk(
  "@user/signUp",
  async (payload: ISignUpRequest, thunkAPI) => {
    try {
      await signUpAPI(payload);
    } catch (err) {
      thunkAPI.rejectWithValue(err);
    }
  }
);

export const getMe = createAsyncThunk("@user/me", async () => {
  try {
    const response = await getMeAPI();
    return response.data.data;
  } catch (err) {
    console.log(err);
  }
});
