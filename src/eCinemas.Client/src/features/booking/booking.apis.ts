import { IAPIResponse } from "~/core/interfaces";
import { ICreateBookingRequest } from "./booking.interfaces";
import { client } from "~/core/client";

const endpoints = {
  createBooking: "/api/booking/create-booking",
  createPayment: "/api/booking/create-payment",
};

export const createBookingAPI = (payload: ICreateBookingRequest) =>
  client.request<IAPIResponse<any>>({
    method: "POST",
    url: endpoints.createBooking,
    data: payload,
  });
