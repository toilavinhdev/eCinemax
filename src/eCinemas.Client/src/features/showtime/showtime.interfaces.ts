export interface IShowTimeState {
  list: ICinemaShowTime[];
  loadingList?: boolean;
}

export interface IListShowTimeRequest {
  movieId: string;
  date: Date;
}

export interface ICinemaShowTime {
  cinemaId: string;
  cinemaName: string;
  cinemaAddress: string;
  showTimes: IShowTimeValue[];
}

export interface IShowTimeValue {
  showTimeId: string;
  startAt: Date;
  available: number;
}
