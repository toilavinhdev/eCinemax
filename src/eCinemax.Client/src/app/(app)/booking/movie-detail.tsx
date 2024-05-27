import { FontAwesome } from "@expo/vector-icons";
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
import {
  EMovieStatus,
  clearMovie,
  clearReviews,
  getMovie,
  listReview,
  markMovie,
  refreshStatus,
} from "~/features/movie";
import { useAppDispatch, useAppSelector } from "~/features/store";
import { ButtonComponent, NoDataComponent } from "~/shared/components";
import { colors } from "~/shared/constants";

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
        <View className="flex-row gap-x-4">
          <Image
            source={{ uri: movie?.posterUrl }}
            className="w-[110px] h-[140px] rounded-lg"
          />

          <View className="flex-1 space-y-2">
            <Text
              className="font-bold text-[16px]"
              style={{ color: colors.primary }}
            >
              {movie?.title}
            </Text>
            <View className="flex-row gap-x-1">
              {movie?.genres.map((g, idx) => (
                <View key={idx} className="bg-gray-700 px-1 py-[1px] rounded">
                  <Text className="text-white text-[13px]">{g}</Text>
                </View>
              ))}
            </View>
            <View className="flex-row gap-x-1">
              <View className="bg-green-800  px-1 py-[1px] rounded">
                <Text className="text-white text-[13px]">{movie?.age}+</Text>
              </View>
              <View className="bg-green-800  px-1 py-[1px] rounded">
                <Text className="text-white text-[13px">
                  {movie?.durationMinutes} phút
                </Text>
              </View>

              <View className="bg-green-800  px-1 py-[1px] rounded">
                <Text className="text-white text-[13px]">
                  {movie?.languages}
                </Text>
              </View>
            </View>
            <View className="flex-row gap-x-1">
              <View className=" bg-green-800 px-1 py-[1px] rounded">
                <Text className="text-white text-[13px]">
                  {moment(movie?.releasedAt).format("DD/MM/yyyy")}
                </Text>
              </View>
            </View>
            <View className="flex-row gap-x-1">
              <View className="flex-row items-center bg-green-800 gap-x-1 px-1 py-[1px] rounded">
                <StarComponent rate={movie?.averageRate ?? 0} />
                <Text className="text-white text-[13px]">
                  {movie?.averageRate}/10 (
                  {movie?.totalReview.toLocaleString("vi-VN")} reviews)
                </Text>
              </View>
            </View>
          </View>
        </View>

        <View className="mt-5 space-y-3">
          <View className="flex-row">
            <Text className="text-white w-[150px]">Đạo diễn</Text>
            <Text className="text-white flex-1">
              {movie?.directors.reduce((acc, cur) => acc + ", " + cur)}
            </Text>
          </View>
          <View className="flex-row">
            <Text className=" text-white w-[150px]">Diễn viên</Text>
            <Text className="text-white flex-1">
              {movie?.casts.reduce((acc, cur) => acc + ", " + cur)}
            </Text>
          </View>
        </View>

        <View>
          <Text className="text-white mt-4">"{movie?.plot}"</Text>
        </View>

        <View className="bg-gray-700 h-px my-6"></View>

        {movie?.status === EMovieStatus.NowShowing && <ReviewComponent />}
      </ScrollView>

      <View className="flex-row mb-6">
        <TouchableOpacity
          className="p-2 w-[60px] h-[60px] flex items-center justify-center rounded-lg"
          style={{ backgroundColor: colors.primary }}
          onPress={onMarkMovie}
        >
          <FontAwesome
            name={movie?.marked ? "bookmark" : "bookmark-o"}
            size={21}
            color="black"
          />
        </TouchableOpacity>
        <ButtonComponent
          disabled={movie?.status !== EMovieStatus.NowShowing}
          text={
            movie?.status === EMovieStatus.NowShowing ? "Đặt vé" : "Sắp chiếu"
          }
          buttonClassName="flex-1 h-[60px] ml-2"
          textClassName="font-semibold text-[18px]"
          onPress={() =>
            router.push({
              pathname: "/booking/choose-cinema",
              params: { movieId: id },
            })
          }
        />
      </View>
    </View>
  );
};

const ReviewComponent = () => {
  const dispatch = useAppDispatch();
  const { movie, reviews } = useAppSelector((state) => state.movie);
  const { currentUser } = useAppSelector((state) => state.user);

  const loadReviews = () => {
    if (!movie) return;

    dispatch(
      listReview({
        movieId: movie.id,
        pageIndex: 1,
        pageSize: 5,
      })
    );
  };

  useEffect(() => {
    loadReviews();

    return () => {
      dispatch(clearReviews());
    };
  }, []);

  return (
    <View>
      <View className="flex-row justify-between items-center">
        <Text className="text-white text-[14px] font-semibold">Reviews</Text>
        <TouchableOpacity
          onPress={() =>
            router.navigate({
              pathname: "/booking/movie-reviews/",
              params: { movieId: movie?.id },
            })
          }
        >
          <Text className="text-white font-semibold text-[14px]">
            Xem tất cả
          </Text>
        </TouchableOpacity>
      </View>

      {reviews.length === 0 ? (
        <View>
          <Text className="text-white mt-2">Phim chưa có đánh giá</Text>
        </View>
      ) : (
        <ScrollView horizontal className="flex-row gap-x-3 pb-1 mt-2">
          {reviews.map((review) => (
            <View
              key={review.id}
              style={{ backgroundColor: colors.secondary }}
              className="h-[100px] w-[190px] px-3 py-2 rounded-lg"
            >
              <View className="flex-row justify-between items-center">
                <Text
                  className={`text-white text-[13px] font-semibold flex-1 ${currentUser?.id === review.userId ? "text-yellow-500" : ""}`}
                  numberOfLines={1}
                >
                  {review.user}
                </Text>
                <View className="flex-row items-center gap-x-1">
                  <StarComponent rate={review.rate} />
                  <Text className="text-white text-[13px]">{review.rate}</Text>
                </View>
              </View>

              <Text
                className="text-white leading-[21px] mt-1 text-[13px]"
                numberOfLines={3}
              >
                {review.review}
              </Text>
            </View>
          ))}
        </ScrollView>
      )}
    </View>
  );
};

const StarComponent = (props: { rate: number }) => {
  const { rate } = props;

  if (rate >= 0 && rate <= 3)
    return <FontAwesome name="star-o" size={14} color={colors.primary} />;
  if (rate > 3 && rate <= 9.5)
    return (
      <FontAwesome name="star-half-empty" size={14} color={colors.primary} />
    );
  if (rate > 9.5 && rate <= 10)
    return <FontAwesome name="star" size={14} color={colors.primary} />;
};

export default MovieDetailScreen;
