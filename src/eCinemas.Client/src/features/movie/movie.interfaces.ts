import { IPaginationResponse } from "~/core/interfaces";

export interface IMovieState {
  loadingList: boolean;
  loadingGet: boolean;
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
