import { Entypo, MaterialIcons } from "@expo/vector-icons";
import { useLocalSearchParams } from "expo-router";
import moment from "moment";
import React, { useEffect, useState } from "react";
import { RefreshControl, ScrollView, Text, View } from "react-native";
import { getBooking } from "~/features/booking";
import { clearReservations } from "~/features/showtime";
import { useAppDispatch, useAppSelector } from "~/features/store";
import { ButtonComponent, NoDataComponent } from "~/shared/components";
import { colors } from "~/shared/constants";
import { GetSeatName } from "~/shared/utils";

const CheckoutScreen = () => {
  const { bookingId } = useLocalSearchParams<{ bookingId: string }>();
  const booking = useAppSelector((state) => state.booking.booking);
  const dispatch = useAppDispatch();

  const loadBooking = () => {
    if (bookingId) dispatch(getBooking(bookingId));
  };

  useEffect(() => {
    if (!booking) {
      loadBooking();
    }

    return () => {
      dispatch(clearReservations());
    };
  });

  return (
    <View
      style={{ flex: 1, backgroundColor: colors.dark, paddingHorizontal: 10 }}
    >
      <ScrollView
        refreshControl={
          <RefreshControl refreshing={false} onRefresh={loadBooking} />
        }
      >
        <BillComponent />
        <PaymentDurationComponent />
      </ScrollView>
      <PaymentDestinationComponent />
    </View>
  );
};

const BillComponent = () => {
  const booking = useAppSelector((state) => state.booking.booking);

  if (!booking)
    return (
      <View className="flex-1 items-center justify-center">
        <NoDataComponent />
      </View>
    );

  return (
    <View>
      <Text className="text-white mt-10">Hóa đơn</Text>
      <View
        className="p-4 rounded-lg mt-4"
        style={{ backgroundColor: colors.secondary }}
      >
        <View>
          <Text className="text-white text-[18px]">{booking?.cinemaName}</Text>
          <Text className="text-gray-300 text-[12px] mt-1">
            {booking?.cinemaAddress}
          </Text>
        </View>

        <View className="space-y-4 mt-6">
          <View className="flex-row gap-x-4 items-center">
            <MaterialIcons name="movie" size={22} color="white" />
            <Text className="text-white">{booking?.movieTitle}</Text>
          </View>
          <View className="flex-row gap-x-4 items-center">
            <Entypo name="calendar" size={21} color="white" />
            <Text className="text-white">
              {moment(booking?.showTimeStartAt).format(
                "dddd, HH:mm - DD/MM/YYYY"
              )}
            </Text>
          </View>
          <View className="flex-row gap-x-4 items-center">
            <Entypo name="ticket" size={22} color="white" />
            <View className="space-y-1">
              {booking.seats.map((seat) => (
                <Text key={seat.type} className="text-white">
                  {seat.quantity} {GetSeatName(seat.type)}:{" "}
                  {seat.seatNames.join(", ")}
                </Text>
              ))}
            </View>
          </View>
        </View>

        <View className="mt-10">
          <Text className="text-white text-[16px]">Tổng cộng</Text>
          <Text className="text-white font-semibold text-[22px]">
            {booking?.total.toLocaleString("vi-VN")} VND
          </Text>
        </View>
      </View>
    </View>
  );
};

const PaymentDurationComponent = () => {
  const [durationMinutes, setDurationMinutes] = useState(10 * 60);

  useEffect(() => {
    const interval = setInterval(() => {
      setDurationMinutes((pre) => pre - 1);
    }, 1000);

    return () => {
      clearInterval(interval);
    };
  }, []);

  return (
    <View className="mx-auto mt-10">
      <Text className="text-gray-300">
        Thời gian thanh toán còn lại: {durationMinutes} giây
      </Text>
    </View>
  );
};

const PaymentDestinationComponent = () => {
  return (
    <View className="mt-auto mb-[70px]">
      <Text className="text-white mt-10">Phương thức thanh toán</Text>
      <ButtonComponent
        text="VNPay"
        buttonClassName="w-full mt-4 h-[60px]"
        textClassName="text-[18px] font-semibold"
      />
    </View>
  );
};

export default CheckoutScreen;
