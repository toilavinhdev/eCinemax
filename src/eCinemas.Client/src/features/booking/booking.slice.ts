import { createSlice } from "@reduxjs/toolkit";
import { IBookingState } from "./booking.interfaces";
import { createBooking } from "./booking.thunk";

const initialState: IBookingState = {
  status: "idle",
  error: null,
};

const bookingSlice = createSlice({
  name: "@booking",
  initialState: initialState,
  reducers: {},
  extraReducers: (builder) => {
    builder.addCase(createBooking.pending, (state) => {
      state.status = "loading";
      state.error = null;
    });
    builder.addCase(createBooking.fulfilled, (state) => {
      state.status = "success";
    });
    builder.addCase(createBooking.rejected, (state, action) => {
      state.status = "error";
      state.error = action.payload as string;
    });
  },
});

export const {} = bookingSlice.actions;
export default bookingSlice.reducer;
