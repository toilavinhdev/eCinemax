import { createSlice } from "@reduxjs/toolkit";
import { IMovieState } from "~/features/movie/movie.interfaces";
import {
  getCollectionMovie,
  getMovie,
  listMovie,
  markMovie,
} from "~/features/movie/movie.thunk";

const initialState: IMovieState = {
  status: "idle",
  error: null,
  list: [],
  collection: [],
  movie: undefined,
  pagination: undefined,
};

const movieSlice = createSlice({
  name: "@movie",
  initialState: initialState,
  reducers: {
    refreshStatus: (state) => {
      (state.status = "idle"), (state.error = null);
    },
    clearMovie: (state) => {
      state.movie = undefined;
    },
    clearList: (state) => {
      state.list = [];
      state.pagination = undefined;
    },
    clearCollection: (state) => {
      state.collection = [];
      state.pagination = undefined;
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
      state.list =
        pagination.pageIndex === 1 ? records : [...state.list, ...records];
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
    builder.addCase(getMovie.rejected, (state, action) => {
      state.status = "failed";
      state.error = action.payload as string;
    });
    builder.addCase(markMovie.pending, (state) => {
      state.status = "loading";
      state.error = null;
    });
    builder.addCase(markMovie.fulfilled, (state) => {
      state.status = "success";
      if (state.movie) {
        state.movie.marked = !state.movie.marked;
      }
    });
    builder.addCase(markMovie.rejected, (state, action) => {
      state.status = "failed";
      state.error = action.payload as string;
    });
    builder.addCase(getCollectionMovie.pending, (state) => {
      state.status = "loading";
      state.error = null;
    });
    builder.addCase(getCollectionMovie.fulfilled, (state, action) => {
      state.status = "success";
      const { records, pagination } = action.payload;
      state.collection =
        pagination.pageIndex === 1
          ? records
          : [...state.collection, ...records];
      state.collectionPagination = pagination;
    });
    builder.addCase(getCollectionMovie.rejected, (state) => {
      state.status = "failed";
    });
  },
});

export const { clearList, refreshStatus, clearMovie } = movieSlice.actions;
export default movieSlice.reducer;
