import { IPaginationResponse } from "~/core/interfaces";

export interface IMovieState {
  loading?: boolean;
  list: IMovieViewModel[];
  movie?: IGetMovieResponse;
}

export interface IListMovieRequest {
  pageIndex: number;
  pageSize: number;
  status: EMovieStatus;
}

export interface IListMovieResponse
  extends IPaginationResponse<IMovieViewModel> {}

export interface IGetMovieResponse {
  id: string;
  title: string;
  description?: string;
  status: EMovieStatus;
  genres?: string[];
  poster: string;
  durationMinutes: number;
}

export interface IMovieViewModel {
  id: string;
  title: string;
  status: EMovieStatus;
  poster: string;
}

export enum EMovieStatus {
  ComingSoon = 0,
  NowShowing,
  StopShowing,
}
