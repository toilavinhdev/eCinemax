import { createSlice } from "@reduxjs/toolkit";
import { INotificationState } from "./notification.interfaces";
import { listNotification } from "./notification.thunk";

const initialState: INotificationState = {
  loading: false,
  error: null,
  list: [],
  pagination: null,
};

const notificationSlice = createSlice({
  name: "@notification",
  initialState,
  reducers: {},
  extraReducers: (builder) => {
    builder.addCase(listNotification.pending, (state) => {
      state.loading = true;
      state.error = null;
    });
    builder.addCase(listNotification.fulfilled, (state, action) => {
      state.loading = false;
      const { records, pagination } = action.payload;
      state.list =
        pagination.pageIndex === 1 ? records : [...state.list, ...records];
      state.pagination = pagination;
    });
    builder.addCase(listNotification.rejected, (state, action) => {
      state.loading = false;
    });
  },
});

export const {} = notificationSlice.actions;
export default notificationSlice.reducer;
