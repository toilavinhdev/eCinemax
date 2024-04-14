import { client } from "~/core/client";
import {
  IGetMovieResponse,
  IListMovieRequest,
  IListMovieResponse,
  IMarkMovieRequest,
} from "~/features/movie/movie.interfaces";
import { IAPIResponse } from "~/core/interfaces";

const endpoints = {
  list: "/api/movie/list",
  get: "/api/movie/get",
  mark: "/api/movie/mark",
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

export const markMovieAPI = (payload: IMarkMovieRequest) =>
  client.request<IAPIResponse<any>>({
    method: "POST",
    url: endpoints.mark,
    data: payload,
  });
