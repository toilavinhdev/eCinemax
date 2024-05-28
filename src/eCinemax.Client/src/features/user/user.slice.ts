import { PayloadAction, createSlice } from "@reduxjs/toolkit";
import {
  getMe,
  signIn,
  signUp,
  updatePassword,
  updateProfile,
} from "~/features/user/user.thunk";
import { IUserState } from "./user.interfaces";

const initialState: IUserState = {
  status: "idle",
  error: null,
  signedIn: false,
  currentUser: null,
};

const userSlice = createSlice({
  name: "@user",
  initialState: initialState,
  reducers: {
    setSignedIn: (state, action: PayloadAction<boolean>) => {
      state.signedIn = action.payload;
    },
    refreshStatus: (state) => {
      state.status = "idle";
      state.error = null;
    },
    signOut: (state) => {
      state.currentUser = null;
    },
  },
  extraReducers: (builder) => {
    builder.addCase(signIn.pending, (state) => {
      state.status = "loading";
      state.error = null;
    });
    builder.addCase(signIn.fulfilled, (state, action) => {
      state.status = "success";
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
      state.error = null;
      state.status = "loading";
    });
    builder.addCase(getMe.fulfilled, (state, action) => {
      state.currentUser = action.payload;
      state.status = "success";
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
    builder.addCase(updatePassword.rejected, (state, action) => {
      state.status = "failed";
      state.error = action.payload as string;
    });
    builder.addCase(updateProfile.pending, (state) => {
      state.status = "loading";
      state.error = null;
    });
    builder.addCase(updateProfile.fulfilled, (state, action) => {
      state.status = "success";
      state.currentUser = action.payload;
    });
    builder.addCase(updateProfile.rejected, (state, action) => {
      state.status = "failed";
      state.error = action.payload as string;
    });
  },
});

export const { setSignedIn, refreshStatus, signOut } = userSlice.actions;
export default userSlice.reducer;
