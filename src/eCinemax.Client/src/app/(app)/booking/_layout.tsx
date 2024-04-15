import { Stack } from "expo-router";
import React from "react";
import { colors } from "~/shared/constants";

const BookingLayout = () => {
  return (
    <Stack
      screenOptions={{
        headerStyle: { backgroundColor: colors.secondary },
        headerTintColor: "white",
      }}
    >
      <Stack.Screen name="movie-detail" options={{ title: "Chi tiết phim" }} />
      <Stack.Screen
        name="choose-cinema"
        options={{ title: "Chọn rạp chiếu" }}
      />
      <Stack.Screen name="choose-seats" options={{ title: "Chọn ghế" }} />
      <Stack.Screen
        name="checkout"
        options={{
          title: "Thanh toán",
        }}
      />
      <Stack.Screen
        name="history-transactions"
        options={{
          title: "Lịch sử giao dịch",
        }}
      />
    </Stack>
  );
};

export default BookingLayout;
