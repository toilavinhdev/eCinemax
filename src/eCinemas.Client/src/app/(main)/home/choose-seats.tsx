import { useLocalSearchParams } from "expo-router";
import React, { useEffect, useState } from "react";
import { StyleSheet, Text, TouchableOpacity, View } from "react-native";
import {
  ESeatStatus,
  ESeatType,
  IReservation,
  ISeatPrice,
  removeReservation,
  getShowtime,
  reservation,
  clearReservations,
} from "~/features/showtime";
import { useAppDispatch, useAppSelector } from "~/features/store";
import { ButtonComponent } from "~/shared/components";
import { colors } from "~/shared/constants";

const ChooseSeatsScreen = () => {
  const { showtimeId } = useLocalSearchParams<{ showtimeId: string }>();
  const dispatch = useAppDispatch();
  const showtime = useAppSelector((state) => state.showtime.showtime);

  useEffect(() => {
    if (showtimeId) dispatch(getShowtime(showtimeId));

    return () => {
      dispatch(clearReservations());
    };
  }, [showtimeId]);

  return (
    <View className="flex-1 px-2" style={{ backgroundColor: colors.dark }}>
      <ScreenComponent />
      <RoomComponent reservations={showtime?.reservations ?? []} />
      <SeatInfoComponent tickets={showtime?.ticket} />
      <TotalComponent />
    </View>
  );
};

const ScreenComponent = () => {
  return (
    <View className="mt-3">
      <Text className="text-white text-center text-[12px]">Screen</Text>
      <View
        className="w-full h-[12px] rounded-b-lg"
        style={{ backgroundColor: colors.secondary }}
      ></View>
    </View>
  );
};

const RoomComponent = (props: { reservations: IReservation[][] }) => {
  const { reservations } = props;
  return (
    <View className="mt-4">
      {reservations.map((row, idx) => (
        <View key={idx} className="flex-row">
          {row.map((seat) => (
            <SeatComponent key={seat.name} reservation={seat} />
          ))}
        </View>
      ))}
    </View>
  );
};

const SeatComponent = (props: { reservation: IReservation }) => {
  const { name, status, type } = props.reservation;
  const [selected, setSelected] = useState<boolean>(false);
  const dispatch = useAppDispatch();
  const styles = StyleSheet.create({
    baseContainer: {
      flex: 1,
      margin: 4,
      paddingVertical: 6,
      backgroundColor:
        status === ESeatStatus.SoldOut
          ? colors.success
          : selected
            ? colors.primary
            : colors.gray,
    },
    baseText: {
      color: status === ESeatStatus.SoldOut ? "white" : "black",
      textAlign: "center",
      fontSize: 13,
    },
  });
  const apperance =
    type === ESeatType.Normal
      ? "rounded-lg"
      : type === ESeatType.VIP
        ? "rounded-full"
        : type === ESeatType.Couple
          ? "rounded-t-2xl"
          : "";

  const onSelect = () => {
    if (status === ESeatStatus.SoldOut) return;
    if (!selected) {
      dispatch(reservation(props.reservation));
    } else {
      dispatch(removeReservation(props.reservation));
    }
    setSelected(!selected);
  };

  return (
    <TouchableOpacity
      onPress={() => onSelect()}
      style={styles.baseContainer}
      className={apperance}
    >
      <Text style={styles.baseText}>{name}</Text>
    </TouchableOpacity>
  );
};

const TotalComponent = () => {
  const reservations = useAppSelector((x) => x.showtime.reservations);
  const showtime = useAppSelector((x) => x.showtime.showtime);
  const [total, setTotal] = useState<number>(0);

  useEffect(() => {
    const amount =
      reservations?.reduce(
        (acc, cur) =>
          acc + (showtime?.ticket.find((x) => x.type === cur.type)?.price ?? 0),
        0
      ) ?? 0;
    setTotal(amount);
  });

  return (
    <View className="mt-auto mb-5">
      <Text className="text-white">Total {total.toLocaleString()} VND</Text>
      <ButtonComponent text="BUY TICKETS" buttonClassName="w-full mt-1" />
    </View>
  );
};

const SeatInfoComponent = (props: { tickets?: ISeatPrice[] }) => {
  const { tickets } = props;
  const normalTicket = tickets?.find((x) => x.type === ESeatType.Normal);
  const vipTicket = tickets?.find((x) => x.type === ESeatType.VIP);
  const coupleTicket = tickets?.find((x) => x.type === ESeatType.Couple);
  return (
    <View className="flex items-center mt-3">
      <View className="flex-row gap-2">
        <View className="flex-row gap-1 items-center">
          <View
            className="h-[14px] w-[14px] rounded"
            style={{ backgroundColor: colors.success }}
          ></View>
          <Text className="text-white text-[10px]">Sold out</Text>
        </View>
        <View className="flex-row gap-1 items-center">
          <View
            className="h-[14px] w-[14px] rounded"
            style={{ backgroundColor: colors.primary }}
          ></View>
          <Text className="text-white text-[10px]">Selected</Text>
        </View>
      </View>
      <View className="flex-row gap-2">
        {normalTicket && (
          <View className="flex-row gap-1 items-center">
            <View
              className="h-[14px] w-[14px] rounded"
              style={{ backgroundColor: colors.gray }}
            ></View>
            <Text className="text-white text-[10px]">
              Normal ({normalTicket.price})
            </Text>
          </View>
        )}
        {vipTicket && (
          <View className="flex-row gap-1 items-center">
            <View
              className="h-[14px] w-[14px] rounded-full"
              style={{ backgroundColor: colors.gray }}
            ></View>
            <Text className="text-white text-[10px]">
              VIP ({vipTicket?.price})
            </Text>
          </View>
        )}
        {coupleTicket && (
          <View className="flex-row gap-1 items-center">
            <View
              className="h-[14px] w-[18px] rounded-t"
              style={{ backgroundColor: colors.gray }}
            ></View>
            <Text className="text-white text-[10px]">
              Couple ({coupleTicket.price})
            </Text>
          </View>
        )}
      </View>
    </View>
  );
};

export default ChooseSeatsScreen;
