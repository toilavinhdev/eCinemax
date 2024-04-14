import { router, useLocalSearchParams } from "expo-router";
import moment from "moment";
import React, { useEffect } from "react";
import {
  Image,
  RefreshControl,
  ScrollView,
  Text,
  TouchableOpacity,
  View,
} from "react-native";
import { IfComponent } from "~/core/components";
import {
  clearMovie,
  EMovieStatus,
  getMovie,
  markMovie,
  refreshStatus,
} from "~/features/movie";
import { useAppDispatch, useAppSelector } from "~/features/store";
import { ButtonComponent, NoDataComponent } from "~/shared/components";
import { colors } from "~/shared/constants";
import { FontAwesome } from "@expo/vector-icons";

const MovieDetailScreen = () => {
  const { id } = useLocalSearchParams<{ id: string }>();
  const { status, movie } = useAppSelector((state) => state.movie);
  const dispatch = useAppDispatch();

  const loadData = () => {
    if (!id) return;
    dispatch(getMovie(id));
  };

  const onMarkMovie = () => {
    if (id === undefined || movie === undefined) return;
    dispatch(markMovie({ ids: [id], isMark: !movie.marked }));
  };

  useEffect(() => {
    loadData();

    return () => {
      dispatch(refreshStatus());
      dispatch(clearMovie());
    };
  }, []);

  if (!movie) {
    if (status === "idle" || status === "loading")
      return (
        <View
          className="flex-1 p-2"
          style={{ backgroundColor: colors.dark }}
        ></View>
      );
    if (status === "success")
      return (
        <View className="flex-1 p-2" style={{ backgroundColor: colors.dark }}>
          <NoDataComponent></NoDataComponent>
        </View>
      );
    if (status === "failed")
      return (
        <View className="flex-1 p-2" style={{ backgroundColor: colors.dark }}>
          <NoDataComponent text="Có lỗi xảy ra. Vui lòng thử lại"></NoDataComponent>
        </View>
      );
  }

  return (
    <View className="flex-1 p-2" style={{ backgroundColor: colors.dark }}>
      <ScrollView
        className="flex-1 mb-4"
        refreshControl={
          <RefreshControl refreshing={false} onRefresh={loadData} />
        }
        showsVerticalScrollIndicator={false}
      >
        <View className="flex-row items-center justify-between gap-x-3">
          <Text
            className="flex-1 font-semibold text-[16px]"
            style={{ color: colors.primary }}
          >
            {movie?.title}
          </Text>
          <TouchableOpacity className="p-2" onPress={onMarkMovie}>
            <FontAwesome
              name={movie?.marked ? "bookmark" : "bookmark-o"}
              size={21}
              color={colors.primary}
            />
          </TouchableOpacity>
        </View>

        <Image
          source={{ uri: movie?.posterUrl }}
          className="w-[110px] h-[140px] rounded-lg mt-4"
        />

        <Text className="text-white mt-4">"{movie?.plot}"</Text>

        <View className="mt-5 space-y-3">
          <View className="flex-row">
            <Text className="text-white w-[150px]">Đạo diễn</Text>
            <Text className="text-white flex-1">
              {movie?.directors.reduce((acc, cur) => acc + ", " + cur)}
            </Text>
          </View>
          <View className="flex-row">
            <Text className=" text-white w-[150px]">Diễn viên</Text>
            <Text className="text-white flex-1" numberOfLines={1}>
              {movie?.casts.reduce((acc, cur) => acc + ", " + cur)}
            </Text>
          </View>
          <View className="flex-row">
            <Text className=" text-white w-[150px]">Thể loại</Text>
            <Text className="text-white flex-1">
              {movie?.genres.reduce((acc, cur) => acc + ", " + cur)}
            </Text>
          </View>
          <View className="flex-row">
            <Text className=" text-white w-[150px]">Độ tuổi</Text>
            <Text className="text-white flex-1">{movie?.age}+</Text>
          </View>
          <View className="flex-row">
            <Text className=" text-white w-[150px]">Thời lượng</Text>
            <Text className="text-white flex-1">
              {movie?.durationMinutes} Phút
            </Text>
          </View>
          <View className="flex-row">
            <Text className=" text-white w-[150px]">Ngôn ngữ</Text>
            <Text className="text-white flex-1">
              {movie?.languages.reduce((acc, cur) => acc + ", " + cur)}
            </Text>
          </View>
          <View className="flex-row">
            <Text className="text-white w-[150px]">Ngày khởi chiếu</Text>
            <Text className="text-white">
              {moment(movie?.releasedAt).format("DD/MM/yyyy")}
            </Text>
          </View>
        </View>
      </ScrollView>

      <IfComponent condition={movie?.status === EMovieStatus.NowShowing}>
        <ButtonComponent
          text="Đặt vé"
          buttonClassName="w-full mt-auto h-[60px] mb-3"
          textClassName="font-semibold text-[18px]"
          onPress={() =>
            router.push({
              pathname: "/booking/choose-cinema",
              params: { movieId: id },
            })
          }
        />
      </IfComponent>
    </View>
  );
};

export default MovieDetailScreen;
