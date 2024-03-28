import React, { useState } from "react";
import { FlatList, Text, TouchableOpacity, View } from "react-native";
import { colors } from "~/shared/constants";

interface Props {
  onChangeDate?: (val: Date) => void;
}

const DateOfWeekPickerComponent = (props: Props) => {
  const { onChangeDate } = props;
  const currentDate = new Date(Date.now());
  currentDate.setUTCHours(currentDate.getUTCHours() + 7);
  const [selectedDate, setSelectedDate] = useState(currentDate);
  const days: Date[] = [];

  for (let i = currentDate.getDay(); i <= 7; i++) {
    const day = new Date(currentDate);
    day.setDate(currentDate.getDate() + (i - currentDate.getDay()));
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
              {item.getDate() === currentDate.getDate()
                ? "Today"
                : `${item.toLocaleString("en-US", { month: "2-digit" })}-${item.toLocaleString("en-US", { weekday: "short" })}`}
            </Text>
          </TouchableOpacity>
        )}
      />
    </View>
  );
};

export default DateOfWeekPickerComponent;
