import { Entypo, MaterialIcons } from "@expo/vector-icons";
import { router, useLocalSearchParams } from "expo-router";
import moment from "moment";
import React, { useEffect, useState } from "react";
import { Alert, RefreshControl, ScrollView, Text, View } from "react-native";
import {
  EBookingStatus,
  EPaymentDestination,
  checkoutAPI,
  clearBooking,
  getBooking,
  refreshStatus,
} from "~/features/booking";
import { hideGlobalLoading, showGlobalLoading } from "~/features/common";
import { clearReservations } from "~/features/showtime";
import { useAppDispatch, useAppSelector } from "~/features/store";
import { ButtonComponent, NoDataComponent } from "~/shared/components";
import { colors } from "~/shared/constants";
import { GetSeatName } from "~/shared/utils";
import { GetBookingStatusString } from "~/shared/utils/booking.util";
import QRCode from "react-native-qrcode-svg";
import { IfComponent } from "~/core/components";

const CheckoutScreen = () => {
  const { bookingId } = useLocalSearchParams<{ bookingId: string }>();
  const { booking, status, error } = useAppSelector((state) => state.booking);
  const { currentUser } = useAppSelector((state) => state.user);
  const dispatch = useAppDispatch();

  const loadBooking = () => {
    if (bookingId) dispatch(getBooking(bookingId));
  };

  useEffect(() => {
    loadBooking();

    return () => {
      dispatch(clearReservations());
      dispatch(clearBooking());
    };
  }, [bookingId]);

  useEffect(() => {
    dispatch(refreshStatus());
  }, []);

  const buildQrCodeValue = () => {
    return `eCinemax.${currentUser?.email}.${booking?.id}.secret-key`;
  };

  return (
    <View
      className="px-2"
      style={{
        flex: 1,
        backgroundColor: colors.dark,
      }}
    >
      <ScrollView
        refreshControl={
          <RefreshControl refreshing={false} onRefresh={loadBooking} />
        }
      >
        <BillComponent />
        <IfComponent condition={booking?.status === EBookingStatus.Success}>
          <QrCodeComponent value={buildQrCodeValue()} />
        </IfComponent>
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
            <Text className="text-white flex-wrap flex-1">
              {booking?.movieTitle}
            </Text>
          </View>
          <View className="flex-row gap-x-4 items-center">
            <Entypo name="calendar" size={21} color="white" />
            <Text className="text-white">
              {moment(booking?.showTimeStartAt).format(
                "dddd, HH:mm - DD/MM/YYYY"
              )}
            </Text>
          </View>
          {booking.status === EBookingStatus.Success && (
            <View className="flex-row gap-x-4 items-center">
              <MaterialIcons name="theaters" size={22} color="white" />
              <Text className="text-white">{booking.roomName}</Text>
            </View>
          )}
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
          {booking.status !== EBookingStatus.WaitForPay && (
            <View className="flex-row gap-x-4 items-center">
              <MaterialIcons name="payment" size={22} color="white" />
              <Text className="text-white">
                {GetBookingStatusString(booking.status)}
              </Text>
            </View>
          )}
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
  const DURATION = 10 * 60; // 10 minutes

  return (
    <View className="mx-auto mt-10">
      <Text className="text-gray-300">
        Thời gian thanh toán còn lại: {DURATION} giây
      </Text>
    </View>
  );
};

const PaymentDestinationComponent = () => {
  const { booking, status } = useAppSelector((state) => state.booking);
  const [loading, setLoading] = useState(false);
  const dispatch = useAppDispatch();

  const onCheckout = async () => {
    if (!booking) return;
    setLoading(true);
    dispatch(showGlobalLoading());

    try {
      await checkoutAPI({
        bookingId: booking?.id,
        destination: EPaymentDestination.VnPay,
      });

      setLoading(false);
      dispatch(hideGlobalLoading());

      Alert.alert("Thanh toán thành công", "Cảm ơn bạn đã sử dụng dịch vụ", [
        {
          text: "Xem chi tiết",
          onPress: () =>
            router.replace({
              pathname: "/booking/checkout",
              params: { bookingId: booking.id },
            }),
        },
        {
          text: "Về màn hình chính",
          onPress: () => router.replace("/"),
        },
      ]);
    } catch (error: any) {
      setLoading(false);
      dispatch(hideGlobalLoading());
      Alert.alert(error.message);
    }
  };

  useEffect(() => {}, []);

  if (booking?.status !== EBookingStatus.WaitForPay) return;

  return (
    <View className="mt-auto mb-[70px]">
      <ButtonComponent
        text="Thanh toán"
        buttonClassName="w-full mt-4 h-[60px]"
        textClassName="text-[18px] font-semibold"
        disabled={loading}
        loading={loading}
        onPress={onCheckout}
      />
    </View>
  );
};

const QrCodeComponent = (props: { value: string }) => {
  const logoBase64 = "data:image/png;base64,.....";
  const { value } = props;

  return (
    <View className="flex items-center mt-3">
      <View className="bg-white p-4 rounded-lg mt-2">
        <QRCode value={value} size={200} />
      </View>
      <Text className="text-white text-[12px] mt-3">
        Vui lòng đưa mã QR cho nhân viên để check in vé
      </Text>
    </View>
  );
};

export default CheckoutScreen;
