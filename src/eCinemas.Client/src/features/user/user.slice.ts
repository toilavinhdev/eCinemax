import { createSlice } from "@reduxjs/toolkit";
import {
  getMe,
  signIn,
  signUp,
  updatePassword,
} from "~/features/user/user.thunk";
import { IUserState } from "./user.interfaces";

const initialState: IUserState = {
  loadingSignIn: false,
  loadingSignUp: false,
  loadingGetMe: false,
  loadingUpdatePassword: false,
  currentUser: undefined,
};

const userSlice = createSlice({
  name: "@user",
  initialState: initialState,
  reducers: {
    signOut: (state) => {
      state.currentUser = undefined;
    },
  },
  extraReducers: (builder) => {
    builder.addCase(signIn.pending, (state) => {
      state.loadingSignIn = true;
    });
    builder.addCase(signIn.fulfilled, (state, action) => {
      state.loadingSignIn = false;
    });
    builder.addCase(signIn.rejected, (state) => {
      state.loadingSignIn = false;
    });
    builder.addCase(signUp.pending, (state) => {
      state.loadingSignUp = true;
    });
    builder.addCase(signUp.fulfilled, (state) => {
      state.loadingSignUp = false;
    });
    builder.addCase(signUp.rejected, (state) => {
      state.loadingSignUp = false;
    });
    builder.addCase(getMe.pending, (state) => {
      state.loadingGetMe = true;
    });
    builder.addCase(getMe.fulfilled, (state, action) => {
      state.loadingGetMe = false;
      state.currentUser = action.payload;
    });
    builder.addCase(getMe.rejected, (state) => {
      state.loadingGetMe = false;
    });
    builder.addCase(updatePassword.pending, (state) => {
      state.loadingUpdatePassword = true;
    });
    builder.addCase(updatePassword.fulfilled, (state) => {
      state.loadingUpdatePassword = false;
    });
    builder.addCase(updatePassword.rejected, (state) => {
      state.loadingUpdatePassword = false;
    });
  },
});

export const { signOut } = userSlice.actions;
export default userSlice.reducer;
