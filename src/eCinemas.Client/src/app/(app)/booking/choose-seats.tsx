import { router, useLocalSearchParams } from "expo-router";
import React, { useEffect, useState } from "react";
import { StyleSheet, Text, TouchableOpacity, View } from "react-native";
import { IfComponent } from "~/core/components";
import {
  ESeatStatus,
  ESeatType,
  IReservation,
  ISeatPrice,
  addReservation,
  clearReservations,
  clearShowtime,
  getShowtime,
  removeReservation,
  showtimeTotalTicket,
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
      dispatch(clearShowtime());
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
    <View className="mt-10">
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
  const appearance =
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
      dispatch(addReservation(props.reservation));
    } else {
      dispatch(removeReservation(props.reservation));
    }
    setSelected(!selected);
  };

  return (
    <TouchableOpacity
      onPress={() => onSelect()}
      style={styles.baseContainer}
      className={appearance}
    >
      <Text style={styles.baseText}>{name}</Text>
    </TouchableOpacity>
  );
};

const TotalComponent = () => {
  const total = useAppSelector(showtimeTotalTicket);

  return (
    <View className="mt-auto mb-12">
      <ButtonComponent
        text={`Thanh toán(${total.toLocaleString()} VND)`}
        disabled={total === 0}
        buttonClassName="w-full mt-auto mb-[20px] h-[60px]"
        textClassName="text-[18px]"
        onPress={() => router.push("/booking/checkout")}
      />
    </View>
  );
};

const SeatInfoComponent = (props: { tickets?: ISeatPrice[] }) => {
  const { tickets } = props;
  const normalTicket = tickets?.find((x) => x.type === ESeatType.Normal);
  const vipTicket = tickets?.find((x) => x.type === ESeatType.VIP);
  const coupleTicket = tickets?.find((x) => x.type === ESeatType.Couple);
  return (
    <IfComponent condition={!!tickets && tickets.length > 0}>
      <View className="flex items-center mt-6">
        <View className="flex-row gap-2">
          <View className="flex-row gap-1 items-center">
            <View
              className="h-[14px] w-[14px] rounded"
              style={{ backgroundColor: colors.success }}
            ></View>
            <Text className="text-white text-[10px]">Đã hết</Text>
          </View>
          <View className="flex-row gap-1 items-center">
            <View
              className="h-[14px] w-[14px] rounded"
              style={{ backgroundColor: colors.primary }}
            ></View>
            <Text className="text-white text-[10px]">Đang chọn</Text>
          </View>
        </View>
        <View className="flex-row gap-2 mt-1">
          {normalTicket && (
            <View className="flex-row gap-1 items-center">
              <View
                className="h-[14px] w-[14px] rounded"
                style={{ backgroundColor: colors.gray }}
              ></View>
              <Text className="text-white text-[10px]">
                Ghế thường ({normalTicket.price})
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
                Ghế VIP ({vipTicket?.price})
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
                Ghế đôi ({coupleTicket.price})
              </Text>
            </View>
          )}
        </View>
      </View>
    </IfComponent>
  );
};

export default ChooseSeatsScreen;