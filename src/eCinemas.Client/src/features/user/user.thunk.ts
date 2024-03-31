import AsyncStorage from "@react-native-async-storage/async-storage";
import { createAsyncThunk } from "@reduxjs/toolkit";
import { router } from "expo-router";
import {
  getMeAPI,
  signInAPI,
  signUpAPI,
  updatePasswordAPI,
} from "~/features/user/user.apis";
import {
  ISignInRequest,
  ISignUpRequest,
  IUpdatePasswordRequest,
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

export const updatePassword = createAsyncThunk(
  "@user/updatePassword",
  async (payload: IUpdatePasswordRequest, thunkAPI) => {
    try {
      await updatePasswordAPI(payload);
    } catch (err) {}
  }
);
