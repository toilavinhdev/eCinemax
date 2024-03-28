import { createSlice } from "@reduxjs/toolkit";
import { IShowTimeState } from "./showtime.interfaces";
import { listShowtime } from "./showtime.thunk";

const initialState: IShowTimeState = {
  list: [],
};

const showTimeSlice = createSlice({
  name: "@showtime",
  initialState: initialState,
  reducers: {},
  extraReducers: (builder) => {
    builder.addCase(listShowtime.pending, (state) => {
      state.loadingList = true;
    });
    builder.addCase(listShowtime.fulfilled, (state, action) => {
      state.loadingList = false;
      state.list = action.payload!;
    });
    builder.addCase(listShowtime.rejected, (state) => {
      state.loadingList = false;
    });
  },
});

export const {} = showTimeSlice.actions;
export default showTimeSlice.reducer;
