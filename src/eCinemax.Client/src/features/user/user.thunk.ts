import AsyncStorage from "@react-native-async-storage/async-storage";
import { createAsyncThunk } from "@reduxjs/toolkit";
import {
  getMeAPI,
  signInAPI,
  signUpAPI,
  updatePasswordAPI,
  updateProfileAPI,
} from "~/features/user/user.apis";
import {
  ISignInRequest,
  ISignUpRequest,
  IUpdatePasswordRequest,
  IUpdateProfileRequest,
} from "~/features/user/user.interfaces";
import { authConst } from "~/shared/constants";
import { Alert } from "react-native";
import { router } from "expo-router";

export const signIn = createAsyncThunk(
  "@user/signIn",
  async (payload: ISignInRequest, { rejectWithValue }) => {
    try {
      const response = await signInAPI(payload);
      const accessToken = response.data.data.accessToken;
      await AsyncStorage.setItem(authConst.ACCESS_TOKEN, accessToken);
      return accessToken;
    } catch (err: any) {
      return rejectWithValue(err.message);
    }
  }
);

export const signUp = createAsyncThunk(
  "@user/signUp",
  async (payload: ISignUpRequest, { rejectWithValue }) => {
    try {
      await signUpAPI(payload);
    } catch (err: any) {
      return rejectWithValue(err.message);
    }
  }
);

export const getMe = createAsyncThunk(
  "@user/me",
  async (payload: any, { rejectWithValue }) => {
    try {
      const response = await getMeAPI();
      return response.data.data;
    } catch (err: any) {
      return rejectWithValue(err.message);
    }
  }
);

export const updatePassword = createAsyncThunk(
  "@user/updatePassword",
  async (payload: IUpdatePasswordRequest, { rejectWithValue }) => {
    try {
      await updatePasswordAPI(payload);
    } catch (err: any) {
      return rejectWithValue(err.message);
    }
  }
);

export const updateProfile = createAsyncThunk(
  "@user/updateProfile",
  async (payload: IUpdateProfileRequest, { rejectWithValue }) => {
    try {
      const response = await updateProfileAPI(payload);
      return response.data.data;
    } catch (err: any) {
      return rejectWithValue(err.message);
    }
  }
);
