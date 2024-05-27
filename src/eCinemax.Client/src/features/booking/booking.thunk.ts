import { createAsyncThunk } from "@reduxjs/toolkit";
import { router } from "expo-router";
import { Alert } from "react-native";
import {
  checkoutAPI,
  createBookingAPI,
  getBookingAPI,
  listBookingAPI,
} from "./booking.apis";
import {
  ICreateBookingRequest,
  ICreatePaymentRequest,
  IListBookingRequest,
} from "./booking.interfaces";

export const listBooking = createAsyncThunk(
  "@booking/list",
  async (payload: IListBookingRequest, { rejectWithValue }) => {
    try {
      const response = await listBookingAPI(payload);
      return response.data.data;
    } catch (error: any) {
      Alert.alert(error.message);
      return rejectWithValue(error.message as string);
    }
  }
);

export const getBooking = createAsyncThunk(
  "@booking/get",
  async (id: string, { rejectWithValue }) => {
    try {
      const response = await getBookingAPI(id);
      return response.data.data;
    } catch (error: any) {
      Alert.alert(error.message);
      return rejectWithValue(error.message as string);
    }
  }
);

export const createBooking = createAsyncThunk(
  "@booking/create-booking",
  async (payload: ICreateBookingRequest, { rejectWithValue }) => {
    try {
      const response = await createBookingAPI(payload);
      const booking = response.data.data;
      router.replace({
        pathname: "/booking/checkout",
        params: { bookingId: booking.id },
      });
      return booking;
    } catch (error: any) {
      Alert.alert(error.message);
      return rejectWithValue(error.message as string);
    }
  }
);

export const checkout = createAsyncThunk(
  "@booking/checkout",
  async (payload: ICreatePaymentRequest, { rejectWithValue }) => {
    try {
      await checkoutAPI(payload);
      return true;
    } catch (error: any) {
      return rejectWithValue(error.message as string);
    }
  }
);

export const cancelBooking = createAsyncThunk(
  "@booking/cancel",
  async (bookingId: string, { rejectWithValue }) => {
    try {
      await cancelBooking(bookingId);
      return true;
    } catch (error: any) {
      return rejectWithValue(error.message as string);
    }
  }
);
