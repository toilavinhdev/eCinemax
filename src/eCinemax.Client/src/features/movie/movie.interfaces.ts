import { IPagination, IPaginationResponse } from "~/core/interfaces";

export interface IMovieState {
  status: "idle" | "loading" | "success" | "failed";
  error: string | null;
  list: IMovieViewModel[];
  pagination?: IPagination;
  collection: IMovieViewModel[];
  collectionPagination?: IPagination;
  movie?: IGetMovieResponse;
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
