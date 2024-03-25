import { createAsyncThunk } from "@reduxjs/toolkit";
import { getMeAPI, signInAPI, signUpAPI } from "~/features/user/user.apis";
import {
  ISignInRequest,
  ISignUpRequest,
} from "~/features/user/user.interfaces";

export const signIn = createAsyncThunk(
  "@user/signIn",
  async (payload: ISignInRequest, thunkAPI) => {
    try {
      const response = await signInAPI(payload);
      return response.data.data.accessToken;
    } catch (err) {
      thunkAPI.rejectWithValue(err);
    }
  },
);

export const signUp = createAsyncThunk(
  "@user/signUp",
  async (payload: ISignUpRequest, thunkAPI) => {
    try {
      await signUpAPI(payload);
    } catch (err) {
      thunkAPI.rejectWithValue(err);
    }
  },
);

export const getMe = createAsyncThunk("@user/me", async () => {
  try {
    const response = await getMeAPI();
    return response.data.data;
  } catch (err) {
    console.log(err);
  }
});
