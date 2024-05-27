import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import {
  ESeatStatus,
  IReservation,
  IShowTimeState,
} from "./showtime.interfaces";
import { getShowtime, listShowtime } from "./showtime.thunk";
import { RootState } from "../store";

const initialState: IShowTimeState = {
  status: "idle",
  error: null,
  list: [],
  showtime: undefined,
  reservations: [],
  selectedSeatNamesBySomeone: [],
};

const showTimeSlice = createSlice({
  name: "@showtime",
  initialState: initialState,
  reducers: {
    refreshStatus: (state) => {
      (state.status = "idle"), (state.error = null);
    },
    addReservation: (state, action: PayloadAction<IReservation>) => {
      state.reservations?.push(action.payload);
    },
    removeReservation: (state, action: PayloadAction<IReservation>) => {
      state.reservations = state.reservations?.filter(
        (x) => x.name !== action.payload.name
      );
    },
    setSelectedSeatBySomeone: (state, action: PayloadAction<string[]>) => {
      state.selectedSeatNamesBySomeone = action.payload.filter(
        (seatName) => !state.reservations.some((r) => r.name === seatName)
      );
    },
    clearReservations: (state) => {
      state.reservations = [];
    },
    clearListShowtime: (state) => {
      state.list = [];
    },
    clearShowtime: (state) => {
      state.showtime = undefined;
    },
    updateSeatsStatus: (
      state,
      action: PayloadAction<{ seatNames: string[]; newStatus: ESeatStatus }>
    ) => {
      if (state.showtime) {
        state.showtime = {
          ...state.showtime,
          reservations: state.showtime.reservations.map((row) =>
            row.map((seat) => {
              return action.payload.seatNames.includes(seat.name)
                ? { ...seat, status: action.payload.newStatus }
                : seat;
            })
          ),
        };
      }
    },
  },
  extraReducers: (builder) => {
    builder.addCase(listShowtime.pending, (state) => {
      state.status = "loading";
      state.error = null;
    });
    builder.addCase(listShowtime.fulfilled, (state, action) => {
      state.status = "success";
      state.list = action.payload!;
    });
    builder.addCase(listShowtime.rejected, (state) => {
      state.status = "failed";
    });
    builder.addCase(getShowtime.pending, (state) => {
      state.status = "loading";
      state.error = null;
    });
    builder.addCase(getShowtime.fulfilled, (state, action) => {
      state.status = "success";
      state.showtime = action.payload;
    });
    builder.addCase(getShowtime.rejected, (state) => {
      state.status = "failed";
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

export const {
  refreshStatus,
  addReservation,
  removeReservation,
  setSelectedSeatBySomeone,
  clearReservations,
  clearListShowtime,
  clearShowtime,
  updateSeatsStatus,
} = showTimeSlice.actions;
export default showTimeSlice.reducer;
