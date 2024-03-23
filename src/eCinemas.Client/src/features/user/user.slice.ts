import { createSlice } from "@reduxjs/toolkit";
import { getMe, signIn, signUp } from "~/features/user/user.thunk";
import { IUserState } from "./user.interfaces";

const initialState: IUserState = {
  isAuthorized: false,
};

const userSlice = createSlice({
  name: "@user",
  initialState: initialState,
  reducers: {
    signOut: (state) => {
      state.isAuthorized = false;
      state.currentUser = undefined;
    },
  },
  extraReducers: (builder) => {
    builder.addCase(signIn.pending, (state) => {
      state.loading = true;
    });
    builder.addCase(signIn.fulfilled, (state, action) => {
      state.isAuthorized = true;
      state.loading = false;
    });
    builder.addCase(signIn.rejected, (state) => {
      state.isAuthorized = false;
      state.loading = false;
    });
    builder.addCase(signUp.pending, (state) => {
      state.loading = true;
    });
    builder.addCase(signUp.fulfilled, (state) => {
      state.loading = false;
    });
    builder.addCase(signUp.rejected, (state) => {
      state.loading = false;
    });
    builder.addCase(getMe.pending, (state) => {
      state.loading = true;
    });
    builder.addCase(getMe.fulfilled, (state, action) => {
      state.loading = false;
      state.currentUser = action.payload;
    });
    builder.addCase(getMe.rejected, (state) => {
      state.loading = false;
    });
  },
});

export const { signOut } = userSlice.actions;
export default userSlice.reducer;
