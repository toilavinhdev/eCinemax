import { Feather, FontAwesome } from "@expo/vector-icons";
import { useLocalSearchParams } from "expo-router";
import React, { useEffect, useState } from "react";
import { Alert, FlatList, Text, TouchableOpacity, View } from "react-native";
import { IReviewViewModel, listReview, ratingMovie } from "~/features/movie";
import { useAppDispatch, useAppSelector } from "~/features/store";
import { InputComponent, NoDataComponent } from "~/shared/components";
import { colors } from "~/shared/constants";

const MovieReviewScreen = () => {
  const dispatch = useAppDispatch();
  const { reviews, reviewPagination, status, error } = useAppSelector(
    (state) => state.movie
  );
  const { currentUser } = useAppSelector((state) => state.user);
  const { movieId } = useLocalSearchParams<{ movieId: string }>();
  const [pageIndex, setPageIndex] = useState<number>(1);
  const PAGE_SIZE = 12;

  if (!movieId) return;

  const loadData = (batch: number) => {
    dispatch(
      listReview({
        movieId: movieId,
        pageIndex: batch,
        pageSize: PAGE_SIZE,
      })
    );
  };

  const nextBatch = () => {
    if (!reviewPagination?.hasNextPage || status === "loading") return;
    loadData(pageIndex + 1);
    setPageIndex(pageIndex + 1);
  };

  const refresh = () => {
    loadData(1);
    setPageIndex(1);
  };

  useEffect(() => {
    refresh();
  }, [movieId]);

  useEffect(() => {
    if (error) {
      Alert.alert(error, undefined);
    }
  }, [error]);

  return (
    <View className="flex-1 p-2" style={{ backgroundColor: colors.dark }}>
      {reviews[0]?.userId !== currentUser?.id && <RatingComponent />}

      <FlatList
        data={reviews}
        keyExtractor={(x) => x.id}
        renderItem={({ item }) => <ReviewComponent review={item} />}
        onRefresh={refresh}
        refreshing={false}
        onEndReachedThreshold={0}
        onEndReached={nextBatch}
        showsVerticalScrollIndicator={false}
        ListEmptyComponent={() => (
          <NoDataComponent text="Chưa có bình luận nào" />
        )}
        ItemSeparatorComponent={() => <View className="h-[12px]" />}
        className="mt-4"
      />
    </View>
  );
};

const ReviewComponent = (props: { review: IReviewViewModel }) => {
  const { review } = props;
  const { currentUser } = useAppSelector((state) => state.user);

  return (
    <View
      style={{ backgroundColor: colors.secondary }}
      className="h-[100px] w-full px-3 py-2 rounded-lg"
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
  );
};

const RatingComponent = () => {
  const { movieId } = useLocalSearchParams<{ movieId: string }>();
  const dispatch = useAppDispatch();
  const { status } = useAppSelector((state) => state.movie);
  const [rate, setRate] = useState<number>(0);
  const [review, setReview] = useState<string>("");
  const MAX_STAR = 10;
  const value = new Array(MAX_STAR).fill(0).map((_, idx) => idx + 1);

  if (!movieId) return;

  const onRating = () => {
    if (review === "") {
      Alert.alert("Vui lòng nhập nội dung");
      return;
    }

    dispatch(
      ratingMovie({
        movieId,
        rate,
        review,
      })
    );
  };

  return (
    <View>
      <View className="flex-row gap-x-1 justify-center mt-2">
        {value.map((v) => (
          <TouchableOpacity
            key={v}
            onPress={() => setRate(v === 1 && v === rate ? 0 : v)}
          >
            <StarComponent rate={rate >= v ? 10 : 0} size={30} />
          </TouchableOpacity>
        ))}
      </View>
      <View className="flex-row mt-4">
        <InputComponent
          value={review}
          onChangeText={(val) => setReview(val)}
          label="Nhập bình luận"
          inputClassName="text-white"
          labelClassName="text-white"
          maxLength={60}
          containerClassName="flex-1"
        />
        <TouchableOpacity
          style={{ backgroundColor: colors.primary }}
          className="p-2 w-[74px] h-[74px] flex items-center justify-center rounded-lg  ml-3"
          disabled={status === "loading"}
          onPress={onRating}
        >
          <Feather
            name={status === "loading" ? "loader" : "send"}
            size={24}
            color="black"
          />
        </TouchableOpacity>
      </View>
    </View>
  );
};

const StarComponent = (props: { rate: number; size?: number }) => {
  const { rate, size } = props;

  if (rate >= 0 && rate <= 3)
    return (
      <FontAwesome name="star-o" size={size ?? 14} color={colors.primary} />
    );
  if (rate > 3 && rate <= 9.5)
    return (
      <FontAwesome
        name="star-half-empty"
        size={size ?? 14}
        color={colors.primary}
      />
    );
  if (rate > 9.5 && rate <= 10)
    return <FontAwesome name="star" size={size ?? 14} color={colors.primary} />;
};

export default MovieReviewScreen;
