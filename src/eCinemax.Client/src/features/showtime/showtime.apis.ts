import { client } from "~/core/client";
import {
  ICinemaShowTime,
  IGetShowTimeResponse,
  IListShowTimeRequest,
} from "./showtime.interfaces";
import { IAPIResponse } from "~/core/interfaces";

const endpoints = {
  list: "/api/showtime/list",
  get: "/api/showtime/get",
};

export const listShowtimeAPI = (payload: IListShowTimeRequest) =>
  client.request<IAPIResponse<ICinemaShowTime[]>>({
    method: "POST",
    url: endpoints.list,
    data: payload,
  });

export const getShowtimeAPI = (id: string) =>
  client.request<IAPIResponse<IGetShowTimeResponse>>({
    method: "POST",
    url: endpoints.get,
    data: { id },
  });
