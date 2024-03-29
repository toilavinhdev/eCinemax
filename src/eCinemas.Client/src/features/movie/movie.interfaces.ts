import { IPaginationResponse } from "~/core/interfaces";

export interface IMovieState {
  loading: boolean;
  list: IMovieViewModel[];
  selectedMovie?: IMovieViewModel;
}

export interface IListMovieRequest {
  pageIndex: number;
  pageSize: number;
  status: EMovieStatus;
}

export interface IListMovieResponse
  extends IPaginationResponse<IMovieViewModel> {}

export interface IMovieViewModel {
  id: string;
  title: string;
  plot: string;
  directors: string[];
  casts: string[];
  languages: string[];
  status: EMovieStatus;
  genres: string[];
  posterUrl: string;
  released?: any;
  durationMinutes: number;
}

export enum EMovieStatus {
  ComingSoon = 0,
  NowShowing,
  StopShowing,
}
