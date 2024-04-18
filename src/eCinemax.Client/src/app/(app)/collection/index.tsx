import { router } from "expo-router";
import React, { useEffect, useState } from "react";
import { FlatList, Image, Text, TouchableOpacity, View } from "react-native";
import { useSafeAreaInsets } from "react-native-safe-area-context";
import { IfComponent } from "~/core/components";
import {
  EMovieStatus,
  IMovieViewModel,
  getCollectionMovie,
} from "~/features/movie";
import { useAppDispatch, useAppSelector } from "~/features/store";
import { NoDataComponent, SpinnerFooterComponent } from "~/shared/components";
import { colors } from "~/shared/constants";
import { Fontisto } from "@expo/vector-icons";
import { GetMovieStatusString } from "~/shared/utils/booking.util";

const BookMarkScreen = () => {
  const insets = useSafeAreaInsets();
  const dispatch = useAppDispatch();
  const { collection, collectionPagination, status, error } = useAppSelector(
    (state) => state.movie
  );
  const [pageIndex, setPageIndex] = useState<number>(1);
  const PAGE_SIZE = 12;
  const [selectedList, setSelectedList] = useState<string[]>([]);

  const loadData = (idx: number) => {
    dispatch(
      getCollectionMovie({
        pageIndex: idx,
        pageSize: PAGE_SIZE,
        status: EMovieStatus.ComingSoon, //any
        queryMark: true,
      })
    );
  };

  const refresh = () => {
    setPageIndex(1);
    loadData(1);
  };

  const nextBatch = () => {
    if (!collectionPagination?.hasNextPage || status === "loading") return;
    loadData(pageIndex + 1);
    setPageIndex(pageIndex + 1);
  };

  const isMovieSelected = (id: string) => selectedList.some((x) => x == id);

  const selectMovie = (id: string) => {
    if (isMovieSelected(id)) {
      setSelectedList([...selectedList.filter((x) => x !== id)]);
      return;
    }
    setSelectedList([...selectedList, id]);
  };

  useEffect(() => {
    refresh();

    return () => {};
  }, []);

  return (
    <View
      className="flex-1 p-1 pt-2"
      style={{ backgroundColor: colors.dark, paddingBottom: insets.bottom }}
    >
      <FlatList
        data={collection}
        keyExtractor={(item) => item.id}
        renderItem={({ item }) => (
          <View className="flex-row items-center gap-x-3 px-2">
            {/* <TouchableOpacity onPress={() => selectMovie(item.id)}>
              <Fontisto
                name={
                  isMovieSelected(item.id)
                    ? "radio-btn-active"
                    : "radio-btn-passive"
                }
                size={20}
                color={colors.red}
              />
            </TouchableOpacity> */}
            <View className="flex-1">
              <MovieComponent movie={item} />
            </View>
          </View>
        )}
        numColumns={1}
        onRefresh={refresh}
        refreshing={false}
        onEndReachedThreshold={0}
        onEndReached={nextBatch}
        showsVerticalScrollIndicator={false}
        ListEmptyComponent={() => (
          <IfComponent condition={status === "success"}>
            <NoDataComponent />
          </IfComponent>
        )}
        ItemSeparatorComponent={() => (
          <View className="h-px bg-gray-700 my-2" />
        )}
        ListFooterComponent={() => (
          <SpinnerFooterComponent
            show={collectionPagination?.hasNextPage ?? false}
          />
        )}
      />
    </View>
  );
};

const MovieComponent = (props: { movie: IMovieViewModel }) => {
  const { movie } = props;
  return (
    <TouchableOpacity
      className="flex-1"
      onPress={() =>
        router.push({
          pathname: "/booking/movie-detail",
          params: { id: movie.id },
        })
      }
    >
      <View className="flex-row gap-x-2">
        <Image
          source={{ uri: movie.posterUrl }}
          className="h-[50px] w-[45px] rounded-lg"
        />
        <View className="flex-1">
          <Text
            className="flex-1 flex-wrap text-white text-[15px] font-semibold"
            style={{ color: colors.primary }}
          >
            {movie.title}
          </Text>
          <Text className="flex-1 flex-wrap text-gray-200 text-[14px]">
            {GetMovieStatusString(movie.status)}
          </Text>
        </View>
      </View>
    </TouchableOpacity>
  );
};

export default BookMarkScreen;
