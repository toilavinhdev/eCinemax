import { createSlice } from "@reduxjs/toolkit";
import { IMovieState } from "~/features/movie/movie.interfaces";
import { getMovie, listMovie } from "~/features/movie/movie.thunk";

const initialState: IMovieState = {
  status: "idle",
  error: null,
  list: [],
  movie: undefined,
  pagination: undefined,
};

const movieSlice = createSlice({
  name: "@movie",
  initialState: initialState,
  reducers: {
    clearMovie: (state) => {
      state.movie = undefined;
    },
  },
  extraReducers: (builder) => {
    builder.addCase(listMovie.pending, (state) => {
      state.status = "loading";
      state.error = null;
    });
    builder.addCase(listMovie.fulfilled, (state, action) => {
      state.status = "success";
      const { records, pagination } = action.payload;
      state.list = records;
      state.pagination = pagination;
    });
    builder.addCase(listMovie.rejected, (state) => {
      state.status = "failed";
    });
    builder.addCase(getMovie.pending, (state) => {
      state.status = "loading";
      state.error = null;
    });
    builder.addCase(getMovie.fulfilled, (state, action) => {
      state.status = "success";

      state.movie = action.payload;
    });
    builder.addCase(getMovie.rejected, (state) => {
      state.status = "failed";
    });
  },
});

export const { clearMovie } = movieSlice.actions;
export default movieSlice.reducer;
