import { IPagination, IPaginationResponse } from "~/core/interfaces";
import { ESeatType } from "../showtime";

export interface IBookingState {
  status: "idle" | "loading" | "success" | "error";
  error: string | null;
  booking: IGetBookingResponse | null;
  list: IBookingViewModel[];
  pagination: IPagination | undefined;
}

export interface IGetBookingResponse {
  id: string;
  movieTitle: string;
  moviePosterUrl: string;
  cinemaName: string;
  cinemaAddress: string;
  roomName: string;
  seats: IBookingSeat[];
  total: number;
  showTimeStartAt: Date;
  showTimeFinishAt: Date;
  paymentExpiredAt: Date;
  createdAt: Date;
  status: EBookingStatus;
}

export interface IBookingSeat {
  type: ESeatType;
  seatNames: string[];
  quantity: string;
}

export interface ICreateBookingRequest {
  showTimeId: string;
  seatNames: string[];
}

export interface ICreateBookingResponse {
  id: string;
  showTimeId: string;
  seatNames: string[];
  total: number;
  status: EBookingStatus;
  paymentExpiryAt: Date;
}

export interface ICreatePaymentRequest {
  bookingId: string;
  destination: EPaymentDestination;
}

export interface IListBookingRequest {
  pageIndex: number;
  pageSize: number;
  status: EBookingStatus;
}

export interface IListBookingResponse
  extends IPaginationResponse<IBookingViewModel> {}

export interface IBookingViewModel {
  id: string;
  seats: IBookingSeat[];
  total: number;
  status: EBookingStatus;
  createdAt: Date;
  movieId: string;
  movieName: string;
  moviePosterUrl: string;
}

export enum EBookingStatus {
  WaitForPay = 0,
  Success,
  Cancelled,
  Failed,
  Expired,
}

export enum EPaymentDestination {
  VnPay = 0,
  Momo,
}
