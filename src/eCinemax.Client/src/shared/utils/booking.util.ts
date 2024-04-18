import { EBookingStatus } from "~/features/booking";
import { EMovieStatus } from "~/features/movie";

export const GetMovieStatusString = (status: EMovieStatus) => {
  switch (status) {
    case EMovieStatus.NowShowing:
      return "Đang chiếu";
    case EMovieStatus.ComingSoon:
      return "Sắp ra rạp";
    case EMovieStatus.StopShowing:
      return "Không có lịch chiếu";
    default:
      throw new Error("Out of range seat type value");
  }
};

export const GetBookingStatusString = (status: EBookingStatus) => {
  switch (status) {
    case EBookingStatus.WaitForPay:
      return "Chờ thanh toán";
    case EBookingStatus.Failed:
      return "Thất bại";
    case EBookingStatus.Expired:
    case EBookingStatus.Cancelled:
      return "Đã hủy";
    case EBookingStatus.Success:
      return "Thành công";
    default:
      throw new Error("Out of range seat type value");
  }
};
