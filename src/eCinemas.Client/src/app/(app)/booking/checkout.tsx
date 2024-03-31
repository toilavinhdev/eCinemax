import React, { useEffect, useState } from "react";
import { Text, View } from "react-native";
import { IReservation, showtimeTotalTicket } from "~/features/showtime";
import { useAppSelector } from "~/features/store";
import { ButtonComponent } from "~/shared/components";
import { colors } from "~/shared/constants";

interface IGroupedReservations {
  [key: number]: IReservation[];
}

const CheckoutScreen = () => {
  const [groupedReservations, setGroupedReservations] =
    useState<IGroupedReservations>({});
  const currentShowtime = useAppSelector((state) => state.showtime.showtime);
  const reservations = useAppSelector((state) => state.showtime.reservations);
  const total = useAppSelector(showtimeTotalTicket);
  const cinemaName = currentShowtime?.cinemaName;
  const startAt = new Date(currentShowtime?.startAt ?? new Date());

  useEffect(() => {
    const handleGroupByReservation = () => {};

    handleGroupByReservation();
  }, [groupedReservations]);

  return (
    <View
      style={{ flex: 1, backgroundColor: colors.dark, paddingHorizontal: 10 }}
    >
      <Text className="text-white mt-10">Hóa đơn</Text>
      <View
        className="p-4 rounded-lg mt-4 space-y-3"
        style={{ backgroundColor: colors.secondary }}
      >
        <Text className="text-white">{cinemaName}</Text>
        <Text className="text-white">
          {startAt.toLocaleString("vi-VN", {
            weekday: "long",
            dateStyle: "full",
            timeStyle: "short",
          })}
        </Text>

        <Text className="text-white">{JSON.stringify(reservations)}</Text>

        <Text className="text-white">
          {JSON.stringify(groupedReservations)}
        </Text>

        <View>
          <Text className="text-white">Total</Text>
          <Text className="text-white font-semibold">
            {total.toLocaleString("vi-VN")} VND
          </Text>
        </View>
      </View>
      <View className="mt-10">
        <Text className="text-white mt-10">Phương thức thanh toán</Text>
        <ButtonComponent
          text="VNPay"
          buttonClassName="w-full mt-4 h-[60px]"
          textClassName="text-[18px] font-semibold"
        />
      </View>
    </View>
  );
};

export default CheckoutScreen;
