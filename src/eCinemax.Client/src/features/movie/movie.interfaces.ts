import { IPagination, IPaginationResponse } from "~/core/interfaces";

export interface IMovieState {
  status: "idle" | "loading" | "success" | "failed";
  error: string | null;
  list: IMovieViewModel[];
  pagination?: IPagination;
  collection: IMovieViewModel[];
  collectionPagination?: IPagination;
  movie?: IGetMovieResponse;
  reviews: IReviewViewModel[];
  reviewPagination?: IPagination;
}

export interface IListMovieRequest {
  pageIndex: number;
  pageSize: number;
  status: EMovieStatus;
  queryMark?: boolean;
}

export interface IListMovieResponse
  extends IPaginationResponse<IMovieViewModel> {}

export interface IGetMovieResponse {
  id: string;
  title: string;
  plot: string;
  directors: string[];
  casts: string[];
  age: number;
  languages: string[];
  status: EMovieStatus;
  genres: string[];
  posterUrl: string;
  releasedAt?: Date;
  durationMinutes: number;
  marked: boolean;
  averageRate: number;
  totalReview: number;
  reviews: IReviewViewModel[];
}

export interface IMovieViewModel {
  id: string;
  title: string;
  status: EMovieStatus;
  posterUrl: string;
}

export enum EMovieStatus {
  ComingSoon = 0,
  NowShowing,
  StopShowing,
}

export interface IMarkMovieRequest {
  ids: string[];
  isMark: boolean;
}

export interface IListReviewRequest {
  movieId: string;
  pageIndex: number;
  pageSize: number;
}

export interface IListReviewResponse
  extends IPaginationResponse<IReviewViewModel> {}

export interface IReviewViewModel {
  id: string;
  rate: number;
  user: string;
  review?: string;
  createdAt: Date;
}

export interface ICreateReviewRequest {
  movieId: string;
  rate: number;
  review?: string | null;
}
