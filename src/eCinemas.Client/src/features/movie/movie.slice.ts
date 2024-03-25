import { IMovieState } from "~/features/movie/movie.interfaces";
import { PayloadAction, createSlice } from "@reduxjs/toolkit";
import { listMovie } from "~/features/movie/movie.thunk";

const initialState: IMovieState = {
  list: [],
};

const movieSlice = createSlice({
  name: "@movie",
  initialState: initialState,
  reducers: {
    selectMovie: (state, action: PayloadAction<string>) => {
      state.selectedMovie = state.list.find((x) => x.id === action.payload);
    },
  },
  extraReducers: (builder) => {
    builder.addCase(listMovie.pending, (state) => {
      state.loading = true;
    });
    builder.addCase(listMovie.fulfilled, (state, action) => {
      state.loading = false;
      state.list = action.payload!.records;
    });
    builder.addCase(listMovie.rejected, (state) => {
      state.loading = false;
    });
  },
});

export const { selectMovie } = movieSlice.actions;
export default movieSlice.reducer;
