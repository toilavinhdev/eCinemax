import { client } from "~/core/client";
import { IAPIResponse } from "~/core/interfaces";
import { ICinemaViewModel } from "./cinema.interfaces";

const endpoints = {
  list: "/api/cinema/list",
};

export const listCinemaAPI = () =>
  client.request<IAPIResponse<ICinemaViewModel[]>>({
    method: "POST",
    url: endpoints.list,
  });
