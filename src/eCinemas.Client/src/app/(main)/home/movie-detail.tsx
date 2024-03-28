import { router } from "expo-router";
import React from "react";
import { Image, ScrollView, Text, View } from "react-native";
import { EMovieStatus } from "~/features/movie";
import { useAppSelector } from "~/features/store";
import { ButtonComponent } from "~/shared/components";
import { colors } from "~/shared/constants";

const MovieDetailScreen = () => {
  const movie = useAppSelector((state) => state.movie.selectedMovie);

  return (
    <ScrollView className="flex-1 p-2" style={{ backgroundColor: colors.dark }}>
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
      {movie?.status === EMovieStatus.NowShowing && (
        <ButtonComponent
          text="BUY TICKET"
          buttonClassName="w-full mt-8"
          textClassName="font-semibold"
          onPress={() =>
            router.push({
              pathname: "/(main)/home/choose-cinema",
              params: { movieId: movie?.id },
            })
          }
        />
      )}
    </ScrollView>
  );
};

export default MovieDetailScreen;
