import { createAsyncThunk } from "@reduxjs/toolkit";
import {
  getMovieAPI,
  listMovieAPI,
  listReviewAPI,
  markMovieAPI,
  ratingMovieAPI,
} from "~/features/movie/movie.apis";
import {
  ICreateReviewRequest,
  IListMovieRequest,
  IListReviewRequest,
  IMarkMovieRequest,
} from "~/features/movie/movie.interfaces";

export const listMovie = createAsyncThunk(
  "@movie/list",
  async (payload: IListMovieRequest, { rejectWithValue }) => {
    try {
      const response = await listMovieAPI(payload);
      return response.data.data;
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

export const getCollectionMovie = createAsyncThunk(
  "@movie/collection",
  async (payload: IListMovieRequest, { rejectWithValue }) => {
    try {
      const response = await listMovieAPI(payload);
      return response.data.data;
    } catch (error: any) {
      return rejectWithValue(error.message);
    }
  }
);

export const markMovie = createAsyncThunk(
  "@movie/mark",
  async (payload: IMarkMovieRequest, thunkAPI) => {
    try {
      const response = await markMovieAPI(payload);
      return response.data.data;
    } catch (error: any) {
      return thunkAPI.rejectWithValue(error.message);
    }
  }
);

export const ratingMovie = createAsyncThunk(
  "@movie/rating",
  async (payload: ICreateReviewRequest, thunkAPI) => {
    try {
      const response = await ratingMovieAPI(payload);
      return response.data.data;
    } catch (error: any) {
      return thunkAPI.rejectWithValue(error.message);
    }
  }
);

export const listReview = createAsyncThunk(
  "@movie/reviews",
  async (payload: IListReviewRequest, thunkAPI) => {
    try {
      const response = await listReviewAPI(payload);
      return response.data.data;
    } catch (error: any) {
      return thunkAPI.rejectWithValue(error.message);
    }
  }
);
