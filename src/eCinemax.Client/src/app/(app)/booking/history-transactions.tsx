import { View, Text, FlatList, TouchableOpacity, Image } from "react-native";
import React, { Dispatch, SetStateAction, useEffect, useState } from "react";
import { colors } from "~/shared/constants";
import {
  EBookingStatus,
  IBookingViewModel,
  clearListBooking,
  listBooking,
  refreshStatus,
} from "~/features/booking";
import { useAppDispatch, useAppSelector } from "~/features/store";
import { GetBookingStatusString } from "~/shared/utils/booking.util";
import { NoDataComponent, SpinnerFooterComponent } from "~/shared/components";
import { GetSeatName } from "~/shared/utils";
import moment from "moment";
import { hideGlobalLoading, showGlobalLoading } from "~/features/common";
import { router } from "expo-router";

const HistoryTransactionsScreen = () => {
  const dispatch = useAppDispatch();
  const { list, pagination, status, error } = useAppSelector(
    (state) => state.booking
  );
  const [bookingStatusFilter, setBookingStatusFilter] =
    useState<EBookingStatus>(EBookingStatus.WaitForPay);
  const [pageIndex, setPageIndex] = useState<number>(1);
  const PAGE_SIZE = 12;

  const loadData = (batch: number) => {
    dispatch(
      listBooking({
        pageIndex: batch,
        pageSize: PAGE_SIZE,
        status: bookingStatusFilter,
      })
    );
  };

  const refresh = () => {
    loadData(1);
    setPageIndex(1);
  };

  const nextBatch = () => {
    if (status === "loading" || !pagination?.hasNextPage) return;
    loadData(pageIndex + 1);
    setPageIndex(pageIndex + 1);
  };

  useEffect(() => {
    refresh();

    return () => {
      dispatch(refreshStatus());
      dispatch(clearListBooking());
    };
  }, []);

  useEffect(() => {
    refresh();
  }, [bookingStatusFilter]);

  useEffect(() => {
    if (status === "loading") {
      dispatch(showGlobalLoading());
    } else {
      dispatch(hideGlobalLoading());
    }
  }, [status]);

  return (
    <View className="flex-1" style={{ backgroundColor: colors.dark }}>
      <BookingStatusComponent
        currentStatus={bookingStatusFilter}
        setStatus={setBookingStatusFilter}
      />
      <FlatList
        data={list}
        keyExtractor={(x) => x.id}
        renderItem={({ item }) => (
          <TouchableOpacity
            onPress={() =>
              router.push({
                pathname: "/booking/checkout",
                params: { bookingId: item.id },
              })
            }
            className="mx-2"
          >
            <BookingComponent booking={item} />
          </TouchableOpacity>
        )}
        onRefresh={refresh}
        refreshing={false}
        onEndReachedThreshold={0}
        onEndReached={nextBatch}
        ListEmptyComponent={() => <NoDataComponent />}
        ItemSeparatorComponent={() => <View className="h-[20px]" />}
        ListFooterComponent={() => (
          <SpinnerFooterComponent show={pagination?.hasNextPage ?? false} />
        )}
      />
    </View>
  );
};

const BookingComponent = (props: { booking: IBookingViewModel }) => {
  const { booking } = props;
  return (
    <View className="flex-row gap-x-3">
      <Image
        source={{ uri: booking.moviePosterUrl }}
        className="w-[80px] h-[105px]"
      />
      <View className="flex-1">
        <Text
          className="text-white font-semibold text-[15px] flex-1 flex-wrap"
          numberOfLines={2}
        >
          {booking.movieName}
        </Text>
        <Text className="text-white text-[13px] mt-2">
          Loại vé:{" "}
          {booking.seats
            .reduce<
              string[]
            >((acc, cur) => [...acc, `${cur.quantity} ${GetSeatName(cur.type)}`], [])
            .join(", ")}
        </Text>
        <Text className="text-white text-[13px]">
          Tổng cộng: {booking.total.toLocaleString("vi-VN")} VND
        </Text>
        <Text className="text-white text-[13px]">
          Trạng thái: {GetBookingStatusString(booking.status)}
        </Text>
        <Text className="text-white text-[13px]">
          Thời gian: {moment(booking.createdAt).format("HH:mm DD/MM/yyyy")}
        </Text>
      </View>
    </View>
  );
};

const BookingStatusComponent = (props: {
  currentStatus: EBookingStatus;
  setStatus: Dispatch<SetStateAction<EBookingStatus>>;
}) => {
  const { currentStatus, setStatus } = props;

  const Component = (props: { status: EBookingStatus; text: string }) => {
    const { status, text } = props;
    return (
      <TouchableOpacity onPress={() => setStatus(status)} className="flex-1">
        <Text
          style={{
            color: currentStatus === status ? colors.primary : "white",
          }}
          className={`text-center text-[16px] py-6 ${currentStatus === status ? "font-semibold" : ""}`}
        >
          {text}
        </Text>
      </TouchableOpacity>
    );
  };

  return (
    <View className="flex-row items-center">
      <Component status={EBookingStatus.WaitForPay} text="Chờ thanh toán" />
      <Component status={EBookingStatus.Success} text="Vé đã mua" />
    </View>
  );
};

export default HistoryTransactionsScreen;
