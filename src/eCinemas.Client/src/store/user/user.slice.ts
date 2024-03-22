import { createSlice } from "@reduxjs/toolkit";
import { IUserState } from "./user.interfaces";
import { getMe, signIn, signUp } from "~/store/user/user.thunk";

const initialState: IUserState = {};

const userSlice = createSlice({
  name: "user",
  initialState: initialState,
  reducers: {},
  extraReducers: (builder) => {
    builder.addCase(signIn.pending, (state, action) => {
      state.loading = true;
    });
    builder.addCase(signIn.fulfilled, (state) => {
      state.loading = false;
    });
    builder.addCase(signIn.rejected, (state) => {
      state.loading = false;
    });
    builder.addCase(signUp.pending, (state, action) => {
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

export const {} = userSlice.actions;
export default userSlice.reducer;
