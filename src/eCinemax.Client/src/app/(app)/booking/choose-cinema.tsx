import { router } from "expo-router";
import moment from "moment";
import React, { useEffect, useState } from "react";
import {
  FlatList,
  RefreshControl,
  ScrollView,
  Text,
  TouchableOpacity,
  View,
} from "react-native";
import { hideGlobalLoading, showGlobalLoading } from "~/features/common";
import {
  clearListShowtime,
  listShowtime,
  refreshStatus,
} from "~/features/showtime";
import { refreshStatus as bookingRefresh } from "~/features/booking";
import { useAppDispatch, useAppSelector } from "~/features/store";
import { CollapseComponent, NoDataComponent } from "~/shared/components";
import { DateOfWeekPickerComponent } from "~/shared/components/datetimepicker";
import { colors } from "~/shared/constants";

const ChooseCinemaScreen = () => {
  const { movie } = useAppSelector((state) => state.movie);
  const [selectedDate, setSelectedDate] = useState<Date>(new Date(Date.now()));
  const status = useAppSelector((state) => state.showtime.status);
  const dispatch = useAppDispatch();

  const loadData = () => {
    if (!movie) return;
    dispatch(listShowtime({ movieId: movie.id, showDate: selectedDate }));
  };

  useEffect(() => {
    loadData();

    return () => {
      dispatch(clearListShowtime());
      dispatch(refreshStatus());
    };
  }, [selectedDate, movie]);

  useEffect(() => {
    if (status === "loading") {
      dispatch(showGlobalLoading());
    } else {
      dispatch(hideGlobalLoading());
    }
  }, [status]);

  useEffect(() => {
    dispatch(bookingRefresh());
  }, []);

  return (
    <View className="flex-1 p-2" style={{ backgroundColor: colors.dark }}>
      <DateOfWeekPickerComponent
        onChangeDate={(value) => setSelectedDate(value)}
      />
      <ListShowTimeComponent onLoadData={loadData} />
    </View>
  );
};

const ListShowTimeComponent = (props: { onLoadData: () => void }) => {
  const { onLoadData } = props;
  const listOfShowtime = useAppSelector((state) => state.showtime.list);
  const status = useAppSelector((state) => state.showtime.status);

  if (listOfShowtime.length === 0) {
    if (status === "success" || status === "failed") {
      return (
        <ScrollView
          refreshControl={
            <RefreshControl refreshing={false} onRefresh={onLoadData} />
          }
        >
          <NoDataComponent text="Không có lịch chiếu" />
        </ScrollView>
      );
    }
  }

  return (
    <FlatList
      data={listOfShowtime}
      keyExtractor={(item) => item.cinemaId}
      renderItem={({ item, index }) => (
        <CollapseComponent title={item.cinemaName} opend={index === 0}>
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
                    <Text className="text-black text-center font-semibold">
                      {moment(showtime.startAt).format("HH:mm")}
                    </Text>
                    <Text className="text-black text-center text-[12px]">
                      ({showtime.available} vé)
                    </Text>
                  </View>
                </TouchableOpacity>
              </View>
            ))}
          </View>
        </CollapseComponent>
      )}
      onRefresh={onLoadData}
      refreshing={false}
      className="mt-3"
      ItemSeparatorComponent={() => <View className="mt-3" />}
    />
  );
};

export default ChooseCinemaScreen;
