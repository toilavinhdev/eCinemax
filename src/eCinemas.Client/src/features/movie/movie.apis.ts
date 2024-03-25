import { client } from "~/core/client";
import {
  IListMovieRequest,
  IListMovieResponse,
} from "~/features/movie/movie.interfaces";
import { IAPIResponse } from "~/core/interfaces";

const endpoints = {
  list: "/api/movie/list",
};

export const listMovieAPI = (payload: IListMovieRequest) =>
  client.request<IAPIResponse<IListMovieResponse>>({
    method: "POST",
    url: endpoints.list,
    data: payload,
  });
