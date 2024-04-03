import { IAPIResponse } from "~/core/interfaces";
import {
  ICreateBookingRequest,
  IGetBookingResponse,
} from "./booking.interfaces";
import { client } from "~/core/client";

const endpoints = {
  getBooking: "/api/booking/get",
  createBooking: "/api/booking/create",
  checkout: "/api/booking/checkout",
};

export const getBookingAPI = (id: string) =>
  client.request<IAPIResponse<IGetBookingResponse>>({
    method: "POST",
    url: endpoints.getBooking,
    data: { id },
  });

export const createBookingAPI = (payload: ICreateBookingRequest) =>
  client.request<IAPIResponse<IGetBookingResponse>>({
    method: "POST",
    url: endpoints.createBooking,
    data: payload,
  });
