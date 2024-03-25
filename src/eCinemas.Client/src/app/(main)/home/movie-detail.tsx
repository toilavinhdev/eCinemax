import { useLocalSearchParams } from "expo-router";
import React, { useEffect } from "react";
import { Image, Text, View } from "react-native";
import { selectMovie } from "~/features/movie";
import { useAppDispatch, useAppSelector } from "~/features/store";
import { ButtonComponent } from "~/shared/components";
import { colors } from "~/shared/constants";

const MovieDetailComponent = () => {
  const { id } = useLocalSearchParams<{ id: string }>();
  const dispatch = useAppDispatch();
  const movie = useAppSelector((state) => state.movie.selectedMovie);

  useEffect(() => {
    dispatch(selectMovie(id!));
  });

  return (
    <View className="flex-1 p-2" style={{ backgroundColor: colors.dark }}>
      <Text
        className="uppercase font-semibold text-[16px]"
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
          <Text className="text-white">
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
          <Text className="text-white">{movie?.released}</Text>
        </View>
      </View>

      <ButtonComponent text="Buy Ticket" />
    </View>
  );
};

export default MovieDetailComponent;
