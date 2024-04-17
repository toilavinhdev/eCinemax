import { EBookingStatus } from "~/features/booking";

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
