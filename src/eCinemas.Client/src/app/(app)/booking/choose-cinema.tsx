import { router } from "expo-router";
import React, { useEffect, useState } from "react";
import { FlatList, Text, TouchableOpacity, View } from "react-native";
import { clearListShowtime, listShowtime } from "~/features/showtime";
import { useAppDispatch, useAppSelector } from "~/features/store";
import { CollapseComponent, NoDataComponent } from "~/shared/components";
import { DateOfWeekPickerComponent } from "~/shared/components/datetimepicker";
import { colors } from "~/shared/constants";
import { IfComponent } from "~/core/components";

const ChooseCinemaScreen = () => {
  const movie = useAppSelector((state) => state.movie.movie);
  const [selectedDate, setSelectedDate] = useState<Date>(new Date(Date.now()));
  const dispatch = useAppDispatch();
  const listOfShowtime = useAppSelector((state) => state.showtime.list);

  const loadData = () => {
    if (!movie) return;
    dispatch(listShowtime({ movieId: movie.id, showDate: selectedDate }));
  };

  useEffect(() => {
    loadData();

    return () => {
      dispatch(clearListShowtime());
    };
  }, [selectedDate, movie]);

  return (
    <View className="flex-1 p-2" style={{ backgroundColor: colors.dark }}>
      <DateOfWeekPickerComponent
        onChangeDate={(value) => setSelectedDate(value)}
      />
      <IfComponent
        condition={listOfShowtime.length > 0}
        elseTemplate={<NoDataComponent />}
      >
        <FlatList
          data={listOfShowtime}
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
                      onPress={() => {
                        router.push({
                          pathname: "/booking/choose-seats",
                          params: { showtimeId: showtime.showTimeId },
                        });
                      }}
                    >
                      <View className="w-[86px] py-2 ">
                        <Text className="text-black text-center">
                          {new Date(showtime.startAt).toLocaleTimeString(
                            "vi-VN",
                            {
                              hour: "2-digit",
                              minute: "2-digit",
                              hourCycle: "h11",
                            },
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
      </IfComponent>
    </View>
  );
};

const CinemaShowTimeComponent = () => {};

export default ChooseCinemaScreen;
