import { createSlice } from "@reduxjs/toolkit";
import { IBookingState } from "./booking.interfaces";
import {
  cancelBooking,
  checkout,
  createBooking,
  getBooking,
  listBooking,
} from "./booking.thunk";

const initialState: IBookingState = {
  status: "idle",
  error: null,
  booking: null,
  list: [],
  pagination: undefined,
};

const bookingSlice = createSlice({
  name: "@booking",
  initialState: initialState,
  reducers: {
    refreshStatus: (state) => {
      state.status = "idle";
      state.error = null;
    },
    clearBooking: (state) => {
      state.booking = null;
    },
    clearListBooking: (state) => {
      state.list = [];
    },
  },
  extraReducers: (builder) => {
    builder.addCase(listBooking.pending, (state) => {
      state.status = "loading";
      state.error = null;
    });
    builder.addCase(listBooking.fulfilled, (state, action) => {
      state.status = "success";
      const { records, pagination } = action.payload;
      state.list =
        pagination.pageIndex === 1 ? records : [...state.list, ...records];
      state.pagination = pagination;
    });
    builder.addCase(listBooking.rejected, (state, action) => {
      state.status = "error";
      state.error = action.payload as string;
    });
    builder.addCase(createBooking.pending, (state) => {
      state.status = "loading";
      state.error = null;
    });
    builder.addCase(createBooking.fulfilled, (state, action) => {
      state.status = "success";
      state.booking = action.payload;
    });
    builder.addCase(createBooking.rejected, (state, action) => {
      state.status = "error";
      state.error = action.payload as string;
    });
    builder.addCase(getBooking.pending, (state) => {
      state.status = "loading";
      state.error = null;
    });
    builder.addCase(getBooking.fulfilled, (state, action) => {
      state.status = "success";
      state.booking = action.payload;
    });
    builder.addCase(getBooking.rejected, (state, action) => {
      state.status = "error";
      state.error = action.payload as string;
    });
    builder.addCase(checkout.pending, (state) => {
      state.status = "loading";
      state.error = null;
    });
    builder.addCase(checkout.fulfilled, (state, action) => {
      state.status = "success";
    });
    builder.addCase(checkout.rejected, (state, action) => {
      state.status = "error";
      state.error = action.payload as string;
    });
    builder.addCase(cancelBooking.pending, (state) => {
      state.status = "loading";
      state.error = null;
    });
    builder.addCase(cancelBooking.fulfilled, (state, action) => {
      state.status = "success";
    });
    builder.addCase(cancelBooking.rejected, (state, action) => {
      state.status = "error";
      state.error = action.payload as string;
    });
  },
});

export const { refreshStatus, clearBooking, clearListBooking } =
  bookingSlice.actions;
export default bookingSlice.reducer;
