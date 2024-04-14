import { router, useLocalSearchParams } from "expo-router";
import moment from "moment";
import React, { useEffect } from "react";
import { Image, Text, View } from "react-native";
import { IfComponent } from "~/core/components";
import { clearMovie, EMovieStatus, getMovie } from "~/features/movie";
import { useAppDispatch, useAppSelector } from "~/features/store";
import { ButtonComponent } from "~/shared/components";
import { colors } from "~/shared/constants";

const MovieDetailScreen = () => {
  const { id } = useLocalSearchParams<{ id: string }>();
  const { status, movie } = useAppSelector((state) => state.movie);
  const dispatch = useAppDispatch();

  useEffect(() => {
    if (!id) return;
    dispatch(getMovie(id));

    return () => {
      dispatch(clearMovie());
    };
  }, [id]);

  if (status === "loading")
    return (
      <View
        className="flex-1 p-2"
        style={{ backgroundColor: colors.dark }}
      ></View>
    );

  return (
    <View className="flex-1 p-2" style={{ backgroundColor: colors.dark }}>
      <Text
        className="font-semibold text-[16px]"
        style={{ color: colors.primary }}
      >
        {movie?.title}
      </Text>

      <Image
        source={{ uri: movie?.posterUrl }}
        className="w-[110px] h-[140px] rounded-lg mt-4"
      />

      <Text className="text-white mt-4">"{movie?.plot}"</Text>

      <View className="mt-5 space-y-3">
        <View className="flex-row">
          <Text className="uppercase text-white w-[150px]">Đạo diễn</Text>
          <Text className="text-white">
            {movie?.directors.reduce((acc, cur) => acc + ", " + cur)}
          </Text>
        </View>
        <View className="flex-row">
          <Text className="uppercase text-white w-[150px]">Diễn viên</Text>
          <Text className="text-white" numberOfLines={1}>
            {movie?.casts.reduce((acc, cur) => acc + ", " + cur)}
          </Text>
        </View>
        <View className="flex-row">
          <Text className="uppercase text-white w-[150px]">Thể loại</Text>
          <Text className="text-white">
            {movie?.genres.reduce((acc, cur) => acc + ", " + cur)}
          </Text>
        </View>
        <View className="flex-row">
          <Text className="uppercase text-white w-[150px]">Độ tuổi</Text>
          <Text className="text-white">{movie?.age}+</Text>
        </View>
        <View className="flex-row">
          <Text className="uppercase text-white w-[150px]">Thời lượng</Text>
          <Text className="text-white">{movie?.durationMinutes} Phút</Text>
        </View>
        <View className="flex-row">
          <Text className="uppercase text-white w-[150px]">Ngôn ngữ</Text>
          <Text className="text-white">
            {movie?.languages.reduce((acc, cur) => acc + ", " + cur)}
          </Text>
        </View>
        <View className="flex-row">
          <Text className="uppercase text-white w-[150px]">
            Ngày khởi chiếu
          </Text>
          <Text className="text-white">
            {moment(movie?.releasedAt).format("DD/MM/yyyy")}
          </Text>
        </View>
      </View>

      <IfComponent condition={movie?.status === EMovieStatus.NowShowing}>
        <ButtonComponent
          text="Đặt vé"
          buttonClassName="w-full mt-auto mb-[50px] h-[60px]"
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
