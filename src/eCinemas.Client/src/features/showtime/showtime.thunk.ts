import { createAsyncThunk } from "@reduxjs/toolkit";
import { IListShowTimeRequest } from "./showtime.interfaces";
import { listShowtimeAPI } from "./showtime.apis";

export const listShowtime = createAsyncThunk(
  "@showtime/list",
  async (payload: IListShowTimeRequest, thunkAPI) => {
    try {
      const response = await listShowtimeAPI(payload);
      const data = response.data.data;
      return data;
    } catch (error) {
      thunkAPI.rejectWithValue(error);
    }
  }
);
