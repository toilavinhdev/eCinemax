import { createAsyncThunk } from "@reduxjs/toolkit";
import { listNotificationAPI } from "./notification.apis";
import { IListNotificationRequest } from "./notification.interfaces";

export const listNotification = createAsyncThunk(
  "@notification/list",
  async (payload: IListNotificationRequest, { rejectWithValue }) => {
    try {
      const response = await listNotificationAPI(payload);
      return response.data.data;
    } catch (error: any) {
      return rejectWithValue(error.message);
    }
  }
);
