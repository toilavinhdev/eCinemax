import { client } from "~/core/client";
import {
  ICreateReviewRequest,
  IGetMovieResponse,
  IListMovieRequest,
  IListMovieResponse,
  IListReviewRequest,
  IListReviewResponse,
  IMarkMovieRequest,
  IReviewViewModel,
} from "~/features/movie/movie.interfaces";
import { IAPIResponse } from "~/core/interfaces";

const endpoints = {
  list: "/api/movie/list",
  get: "/api/movie/get",
  mark: "/api/movie/mark",
  rating: "/api/movie/rating",
  reviews: "/api/movie/reviews",
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

export const ratingMovieAPI = (payload: ICreateReviewRequest) =>
  client.request<IAPIResponse<IReviewViewModel>>({
    method: "POST",
    url: endpoints.rating,
    data: payload,
  });

export const listReviewAPI = (payload: IListReviewRequest) =>
  client.request<IAPIResponse<IListReviewResponse>>({
    method: "POST",
    url: endpoints.reviews,
    data: payload,
  });
