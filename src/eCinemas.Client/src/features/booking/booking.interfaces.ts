export interface IBookingState {
  status: "idle" | "loading" | "success" | "error";
  error: string | null;
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
