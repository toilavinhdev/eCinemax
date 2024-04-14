import { createAsyncThunk } from "@reduxjs/toolkit";
import { getMovieAPI, listMovieAPI } from "~/features/movie/movie.apis";
import {
  IListMovieRequest,
  IMovieViewModel,
} from "~/features/movie/movie.interfaces";
import { RootState } from "../store";

export const listMovie = createAsyncThunk(
  "@movie/list",
  async (payload: IListMovieRequest, { rejectWithValue, getState }) => {
    try {
      console.log("COMPONENT TO THUNK", payload.pageIndex);

      const response = await listMovieAPI(payload);
      const { records, pagination } = response.data.data;
      const state = getState() as RootState;
      const list = state.movie.list;

      return payload.pageIndex === 1
        ? { pagination, records }
        : { pagination, records: [...list, ...records] };
    } catch (error: any) {
      return rejectWithValue(error.message);
    }
  }
);

export const getMovie = createAsyncThunk(
  "@movie/get",
  async (id: string, thunkAPI) => {
    try {
      const response = await getMovieAPI(id);
      return response.data.data;
    } catch (error: any) {
      return thunkAPI.rejectWithValue(error.message);
    }
  }
);
