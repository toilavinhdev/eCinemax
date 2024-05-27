import { IAPIResponse } from "~/core/interfaces";
import {
  ICreateBookingRequest,
  ICreatePaymentRequest,
  IGetBookingResponse,
  IListBookingRequest,
  IListBookingResponse,
} from "./booking.interfaces";
import { client } from "~/core/client";

const endpoints = {
  list: "/api/booking/list",
  get: "/api/booking/get",
  create: "/api/booking/create",
  checkout: "/api/booking/checkout",
  cancel: "/api/booking/cancel",
};

export const listBookingAPI = (payload: IListBookingRequest) =>
  client.request<IAPIResponse<IListBookingResponse>>({
    method: "POST",
    url: endpoints.list,
    data: payload,
  });

export const getBookingAPI = (id: string) =>
  client.request<IAPIResponse<IGetBookingResponse>>({
    method: "POST",
    url: endpoints.get,
    data: { id },
  });

export const createBookingAPI = (payload: ICreateBookingRequest) =>
  client.request<IAPIResponse<IGetBookingResponse>>({
    method: "POST",
    url: endpoints.create,
    data: payload,
  });

export const checkoutAPI = (payload: ICreatePaymentRequest) =>
  client.request<IAPIResponse<any>>({
    method: "POST",
    url: endpoints.checkout,
    data: payload,
  });

export const cancelBookingAPI = (bookingId: string) =>
  client.request({
    method: "POST",
    url: endpoints.cancel,
    data: {
      id: bookingId,
    },
  });
