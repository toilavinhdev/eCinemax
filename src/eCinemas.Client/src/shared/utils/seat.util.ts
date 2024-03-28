import { ESeatType } from "~/features/showtime";

export const GetSeatName = (type: ESeatType) => {
  switch (type) {
    case ESeatType.Normal:
      return "Normal chair";
    case ESeatType.VIP:
      return "VIP chair";
    case ESeatType.Couple:
      return "Couple chair";
    default:
      return "";
  }
};
