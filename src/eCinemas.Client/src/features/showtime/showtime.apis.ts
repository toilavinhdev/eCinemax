import { client } from "~/core/client";
import { ICinemaShowTime, IListShowTimeRequest } from "./showtime.interfaces";
import { IAPIResponse } from "~/core/interfaces";

const endpoints = {
  list: "/api/showtime/list",
};

export const listShowtimeAPI = (payload: IListShowTimeRequest) =>
  client.request<IAPIResponse<ICinemaShowTime[]>>({
    method: "POST",
    url: endpoints.list,
    data: payload,
  });
