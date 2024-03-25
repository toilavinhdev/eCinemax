import { createAsyncThunk } from "@reduxjs/toolkit";
import { IListMovieRequest } from "~/features/movie/movie.interfaces";
import { listMovieAPI } from "~/features/movie/movie.apis";

export const listMovie = createAsyncThunk(
  "@movie/list",
  async (payload: IListMovieRequest, thunkAPI) => {
    try {
      const response = await listMovieAPI(payload);
      return response.data.data;
    } catch (error) {}
  }
);