import { ESeatType } from "~/features/showtime";

export const GetSeatName = (type: ESeatType) => {
  switch (type) {
    case ESeatType.Normal:
      return "Ghế thường";
    case ESeatType.VIP:
      return "Ghế VIP";
    case ESeatType.Couple:
      return "Ghế đôi";
    default:
      throw new Error("Out of range seat type value");
  }
};
