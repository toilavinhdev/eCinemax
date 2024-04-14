import React, { useState } from "react";
import { FlatList, Text, TouchableOpacity, View } from "react-native";
import { colors } from "~/shared/constants";

interface Props {
  onChangeDate?: (val: Date) => void;
}

const DateOfWeekPickerComponent = (props: Props) => {
  const { onChangeDate } = props;
  const now = Date.now();
  const currentDateTime = new Date(now);
  const [selectedDate, setSelectedDate] = useState<Date>(currentDateTime);
  const days: Date[] = [];

  for (let i = currentDateTime.getDay(); i <= 7; i++) {
    const day = new Date(currentDateTime);
    day.setDate(currentDateTime.getDate() + (i - currentDateTime.getDay()));
    days.push(day);
  }

  const changeDate = (value: Date) => {
    setSelectedDate(value);
    if (onChangeDate) onChangeDate(value);
  };

  return (
    <View>
      <FlatList
        horizontal
        showsHorizontalScrollIndicator={false}
        data={days}
        renderItem={({ item }) => (
          <TouchableOpacity
            onPress={() => changeDate(item)}
            className="rounded-lg mr-2 py-5 w-[70px]"
            style={{
              backgroundColor:
                selectedDate.getDate() == item.getDate()
                  ? colors.primary
                  : colors.secondary,
            }}
          >
            <Text className="text-black text-center text-[22px]">
              {item.getDate()}
            </Text>
            <Text className="text-black text-center mt-1">
              {item.getDate() === currentDateTime.getDate()
                ? "Hôm nay"
                : `${item.toLocaleString("vi-VN", { month: "2-digit" })}-${item.toLocaleString("vi-VN", { weekday: "short" })}`}
            </Text>
          </TouchableOpacity>
        )}
      />
    </View>
  );
};

export default DateOfWeekPickerComponent;
