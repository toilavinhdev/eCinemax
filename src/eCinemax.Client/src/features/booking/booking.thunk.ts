import { createAsyncThunk } from "@reduxjs/toolkit";
import { router } from "expo-router";
import { Alert } from "react-native";
import { createBookingAPI, getBookingAPI } from "./booking.apis";
import { ICreateBookingRequest } from "./booking.interfaces";

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
