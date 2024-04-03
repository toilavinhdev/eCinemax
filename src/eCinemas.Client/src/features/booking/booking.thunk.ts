import { createAsyncThunk } from "@reduxjs/toolkit";
import { router } from "expo-router";
import { Alert } from "react-native";
import { createBookingAPI } from "./booking.apis";
import { ICreateBookingRequest } from "./booking.interfaces";

export const createBooking = createAsyncThunk(
  "@booking/create-booking",
  async (payload: ICreateBookingRequest, { rejectWithValue }) => {
    try {
      const response = await createBookingAPI(payload);
      router.push("/booking/checkout");
      return response.data.data;
    } catch (error: any) {
      Alert.alert(error.message);
      return rejectWithValue(error.message as string);
    }
  }
);
