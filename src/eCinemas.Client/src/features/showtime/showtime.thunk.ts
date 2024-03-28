import { createAsyncThunk } from "@reduxjs/toolkit";
import { IListShowTimeRequest } from "./showtime.interfaces";
import { getShowtimeAPI, listShowtimeAPI } from "./showtime.apis";

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

export const getShowtime = createAsyncThunk(
  "@showtime/get",
  async (id: string, thunkAPI) => {
    try {
      const response = await getShowtimeAPI(id);
      const data = response.data.data;
      return data;
    } catch (error) {
      thunkAPI.rejectWithValue(error);
    }
  }
);
