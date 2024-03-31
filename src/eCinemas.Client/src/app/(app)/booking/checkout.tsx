import React, { useEffect, useState } from "react";
import { Text, View } from "react-native";
import { IReservation, showtimeTotalTicket } from "~/features/showtime";
import { useAppSelector } from "~/features/store";
import { ButtonComponent } from "~/shared/components";
import { colors } from "~/shared/constants";
import { GetSeatName } from "~/shared/utils";
import { Entypo } from "@expo/vector-icons";

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
    const handleGroupByReservation = () => {
      const grouped = reservations.reduce<IGroupedReservations>((acc, cur) => {
        if (!acc[cur.type]) {
          acc[cur.type] = [cur];
        } else {
          acc[cur.type].push(cur);
        }
        return acc;
      }, {});
      setGroupedReservations(grouped);
    };

    handleGroupByReservation();
  }, [reservations]);

  const reservationString = () =>
    Object.keys(groupedReservations).reduce<string>((acc, key, idx, arr) => {
      const name = GetSeatName(Number.parseInt(key));
      const quantity = groupedReservations[Number.parseInt(key)].length;
      return (
        acc + `${quantity} ${name}` + `${arr.length - 1 !== idx ? ", " : ""}`
      );
    }, "");

  return (
    <View
      style={{ flex: 1, backgroundColor: colors.dark, paddingHorizontal: 10 }}
    >
      <Text className="text-white mt-10">Hóa đơn</Text>
      <View
        className="p-4 rounded-lg mt-4 space-y-8"
        style={{ backgroundColor: colors.secondary }}
      >
        <Text className="text-white text-[18px]">{cinemaName}</Text>
        <View className="flex-row gap-x-2 items-center">
          <Entypo name="calendar" size={21} color="white" />
          <Text className="text-white">
            {startAt.toLocaleString("vi-VN", {
              weekday: "long",
              dateStyle: "full",
              timeStyle: "short",
            })}
          </Text>
        </View>
        <View className="flex-row gap-x-2 items-center">
          <Entypo name="ticket" size={24} color="white" />
          <Text className="text-white">{reservationString()}</Text>
        </View>
        <View>
          <Text className="text-white text-[16px]">Total</Text>
          <Text className="text-white font-semibold text-[22px]">
            {total.toLocaleString("vi-VN")} VND
          </Text>
        </View>
      </View>
      <View className="mt-auto mb-[70px]">
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
