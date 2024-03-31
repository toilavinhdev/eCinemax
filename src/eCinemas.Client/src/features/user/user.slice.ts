import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import {
  getMe,
  signIn,
  signUp,
  updatePassword,
} from "~/features/user/user.thunk";
import { IUserState } from "./user.interfaces";

const initialState: IUserState = {
  authenticated: false,
  status: "idle",
  error: null,
  currentUser: null,
};

const userSlice = createSlice({
  name: "@user",
  initialState: initialState,
  reducers: {
    setAuthenticated: (state, action: PayloadAction<boolean>) => {
      state.authenticated = action.payload;
    },
    signOut: (state) => {
      state.currentUser = null;
      state.authenticated = false;
    },
  },
  extraReducers: (builder) => {
    builder.addCase(signIn.pending, (state) => {
      state.status = "loading";
      state.error = null;
    });
    builder.addCase(signIn.fulfilled, (state, action) => {
      state.status = "success";
      state.authenticated = true;
    });
    builder.addCase(signIn.rejected, (state, action) => {
      state.status = "failed";
      state.error = action.payload as string;
    });
    builder.addCase(signUp.pending, (state) => {
      state.status = "loading";
      state.error = null;
    });
    builder.addCase(signUp.fulfilled, (state) => {
      state.status = "success";
    });
    builder.addCase(signUp.rejected, (state, action) => {
      state.status = "failed";
      state.error = action.payload as string;
    });
    builder.addCase(getMe.pending, (state) => {
      state.status = "loading";
      state.error = null;
    });
    builder.addCase(getMe.fulfilled, (state, action) => {
      state.status = "success";
      state.currentUser = action.payload;
    });
    builder.addCase(getMe.rejected, (state) => {
      state.status = "failed";
    });
    builder.addCase(updatePassword.pending, (state) => {
      state.status = "loading";
      state.error = null;
    });
    builder.addCase(updatePassword.fulfilled, (state) => {
      state.status = "success";
    });
    builder.addCase(updatePassword.rejected, (state) => {
      state.status = "failed";
    });
  },
});

export const { setAuthenticated, signOut } = userSlice.actions;
export default userSlice.reducer;
