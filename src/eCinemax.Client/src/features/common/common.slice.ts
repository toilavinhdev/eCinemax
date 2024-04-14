import { createSlice } from "@reduxjs/toolkit";

interface ICommonState {
  globalLoading: boolean;
}

const initialState: ICommonState = {
  globalLoading: false,
};

const commonSlice = createSlice({
  name: "@common",
  initialState,
  reducers: {
    showGlobalLoading: (state) => {
      state.globalLoading = true;
    },
    hideGlobalLoading: (state) => {
      state.globalLoading = false;
    },
  },
});

export const { showGlobalLoading, hideGlobalLoading } = commonSlice.actions;
export default commonSlice.reducer;
