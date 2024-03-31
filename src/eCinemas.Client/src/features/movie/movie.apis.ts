import { client } from "~/core/client";
import {
  IGetMovieResponse,
  IListMovieRequest,
  IListMovieResponse,
} from "~/features/movie/movie.interfaces";
import { IAPIResponse } from "~/core/interfaces";

const endpoints = {
  list: "/api/movie/list",
  get: "/api/movie/get",
};

export const listMovieAPI = (payload: IListMovieRequest) =>
  client.request<IAPIResponse<IListMovieResponse>>({
    method: "POST",
    url: endpoints.list,
    data: payload,
  });

export const getMovieAPI = (id: string) =>
  client.request<IAPIResponse<IGetMovieResponse>>({
    method: "POST",
    url: endpoints.get,
    data: { id },
  });
