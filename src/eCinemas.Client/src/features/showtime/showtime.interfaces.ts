export interface IShowTimeState {
  list: ICinemaShowTime[];
  loadingList: boolean;
  loadingGet: boolean;
  showtime?: IGetShowTimeResponse;
  reservations: IReservation[];
}

export interface IListShowTimeRequest {
  movieId: string;
  showDate: Date;
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

export interface IGetShowTimeResponse {
  id: string;
  movieId: string;
  cinemaName: string;
  startAt: Date;
  ticket: ISeatPrice[];
  reservations: IReservation[][];
}

export interface ISeat {
  row: string;
  column: number;
  name: string;
  type: ESeatType;
}

export interface IReservation extends ISeat {
  status: ESeatStatus;
  reservationAt: Date;
}

export interface ISeatPrice {
  type: ESeatType;
  price: number;
}

export enum ESeatType {
  Empty = 0,
  Normal,
  VIP,
  Couple,
}

export enum ESeatStatus {
  Empty = 0,
  SoldOut,
}
