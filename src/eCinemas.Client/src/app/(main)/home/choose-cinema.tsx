import { router, useLocalSearchParams } from "expo-router";
import React, { useEffect, useState } from "react";
import { FlatList, Text, TouchableOpacity, View } from "react-native";
import { listShowtime } from "~/features/showtime";
import { useAppDispatch, useAppSelector } from "~/features/store";
import { CollapseComponent } from "~/shared/components";
import { DateOfWeekPickerComponent } from "~/shared/components/datetimepicker";
import { colors } from "~/shared/constants";

const ChooseCinemaScreen = () => {
  const { movieId } = useLocalSearchParams<{ movieId: string }>();
  const [selectedDate, setSelectedDate] = useState<Date>(new Date(Date.now()));
  const dispatch = useAppDispatch();
  const showtimes = useAppSelector((state) => state.showtime.list);

  const loadData = () => {
    dispatch(listShowtime({ movieId: movieId!, date: selectedDate }));
  };

  const onSelectShowtime = (showtimeId: string) => {
    router.push({
      pathname: "/(main)/home/choose-seats",
      params: { showtimeId },
    });
  };

  useEffect(() => {
    loadData();
  }, [selectedDate, movieId]);

  return (
    <View className="flex-1 p-2" style={{ backgroundColor: colors.dark }}>
      <DateOfWeekPickerComponent
        onChangeDate={(value) => setSelectedDate(value)}
      />
      <FlatList
        data={showtimes}
        keyExtractor={(item) => item.cinemaId}
        renderItem={({ item }) => (
          <CollapseComponent title={item.cinemaName}>
            <View className="flex-row flex-wrap gap-x-3 gap-y-3 px-5 pb-5">
              {item.showTimes.map((showtime) => (
                <View
                  key={showtime.showTimeId}
                  className="rounded-lg"
                  style={{ backgroundColor: colors.primary }}
                >
                  <TouchableOpacity
                    onPress={() => onSelectShowtime(showtime.showTimeId)}
                  >
                    <View className="w-[86px] py-2 ">
                      <Text className="text-black text-center">
                        {new Date(showtime.startAt).toLocaleTimeString(
                          "en-US",
                          {
                            hour: "2-digit",
                            minute: "2-digit",
                            hourCycle: "h11",
                          }
                        )}
                      </Text>
                    </View>
                  </TouchableOpacity>
                </View>
              ))}
            </View>
          </CollapseComponent>
        )}
        onRefresh={() => loadData()}
        refreshing={false}
        className="mt-3"
        ItemSeparatorComponent={() => <View className="mt-3" />}
      />
    </View>
  );
};

const CinemaShowTimeComponent = () => {};

export default ChooseCinemaScreen;
