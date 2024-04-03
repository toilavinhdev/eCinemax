import { ESeatType } from "../showtime";

export interface IBookingState {
  status: "idle" | "loading" | "success" | "error";
  error: string | null;
  booking: IGetBookingResponse | null;
}

export interface IGetBookingResponse {
  id: string;
  movieTitle: string;
  cinemaName: string;
  cinemaAddress: string;
  seats: IBookingSeat[];
  total: number;
  showTimeStartAt: Date;
  paymentExpiredAt: Date;
  createdAt: Date;
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

export enum EBookingStatus {
  WaitForPay = 0,
  Success,
  Cancelled,
  Failed,
  Expired,
}

export enum EPaymentDestination {
  Processing = 0,
  Success,
  Cancelled,
  Failed,
  Expired,
}
