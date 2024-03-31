import { IMovieState } from "~/features/movie/movie.interfaces";
import { createSlice } from "@reduxjs/toolkit";
import { getMovie, listMovie } from "~/features/movie/movie.thunk";

const initialState: IMovieState = {
  loadingList: false,
  loadingGet: false,
  list: [],
  movie: undefined,
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
      state.loadingList = true;
    });
    builder.addCase(listMovie.fulfilled, (state, action) => {
      state.loadingList = false;
      state.list = action.payload?.records ?? [];
    });
    builder.addCase(listMovie.rejected, (state) => {
      state.loadingList = false;
    });
    builder.addCase(getMovie.pending, (state) => {
      state.loadingGet = true;
    });
    builder.addCase(getMovie.fulfilled, (state, action) => {
      state.loadingGet = false;
      state.movie = action.payload;
    });
    builder.addCase(getMovie.rejected, (state) => {
      state.loadingGet = false;
    });
  },
});

export const { clearMovie } = movieSlice.actions;
export default movieSlice.reducer;
