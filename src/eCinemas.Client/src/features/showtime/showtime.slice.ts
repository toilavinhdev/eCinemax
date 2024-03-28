import { PayloadAction, createSlice } from "@reduxjs/toolkit";
import { IReservation, IShowTimeState } from "./showtime.interfaces";
import { getShowtime, listShowtime } from "./showtime.thunk";
import { RootState } from "../store";

const initialState: IShowTimeState = {
  list: [],
  reservations: [],
};

const showTimeSlice = createSlice({
  name: "@showtime",
  initialState: initialState,
  reducers: {
    reservation: (state, action: PayloadAction<IReservation>) => {
      state.reservations?.push(action.payload);
    },
    removeReservation: (state, action: PayloadAction<IReservation>) => {
      state.reservations = state.reservations?.filter(
        (x) => x.name !== action.payload.name
      );
    },
    clearReservations: (state) => {
      state.reservations = [];
    },
  },
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
    builder.addCase(getShowtime.pending, (state) => {
      state.loadingGet = true;
    });
    builder.addCase(getShowtime.fulfilled, (state, action) => {
      state.loadingGet = false;
      state.showtime = action.payload;
    });
    builder.addCase(getShowtime.rejected, (state) => {
      state.loadingGet = false;
    });
  },
});

export const showtimeTotalTicket = (state: RootState) =>
  state.showtime.reservations?.reduce(
    (acc, cur) =>
      acc +
      (state.showtime.showtime?.ticket.find((x) => x.type === cur.type)
        ?.price ?? 0),
    0
  ) ?? 0;

export const { reservation, removeReservation, clearReservations } =
  showTimeSlice.actions;
export default showTimeSlice.reducer;
