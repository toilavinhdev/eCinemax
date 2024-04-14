import { router } from "expo-router";
import React, { useEffect, useState } from "react";
import {
  FlatList,
  Image,
  Text,
  Touchable,
  TouchableOpacity,
  View,
} from "react-native";
import { useSafeAreaInsets } from "react-native-safe-area-context";
import { IfComponent } from "~/core/components";
import {
  EMovieStatus,
  IMovieViewModel,
  getCollectionMovie,
  listMovie,
} from "~/features/movie";
import { useAppDispatch, useAppSelector } from "~/features/store";
import { NoDataComponent, SpinnerFooterComponent } from "~/shared/components";
import { colors } from "~/shared/constants";

const BookMarkScreen = () => {
  const insets = useSafeAreaInsets();
  const dispatch = useAppDispatch();
  const { collection, collectionPagination, status, error } = useAppSelector(
    (state) => state.movie
  );
  const [pageIndex, setPageIndex] = useState<number>(1);
  const PAGE_SIZE = 12;

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

  useEffect(() => {
    refresh();

    return () => {};
  }, []);

  return (
    <View
      className="flex-1 p-1"
      style={{ backgroundColor: colors.dark, paddingBottom: insets.bottom }}
    >
      <FlatList
        data={collection}
        keyExtractor={(item) => item.id}
        renderItem={({ item }) => <MovieComponent movie={item} />}
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
      onPress={() =>
        router.push({
          pathname: "/booking/movie-detail",
          params: { id: movie.id },
        })
      }
    >
      <View className="flex-row gap-x-4">
        <Image
          source={{ uri: movie.posterUrl }}
          className="h-[80px] w-[60px] rounded-lg"
        />
        <View className="flex-1">
          <Text className="flex-1 flex-wrap text-white">{movie.title}</Text>
        </View>
      </View>
    </TouchableOpacity>
  );
};

export default BookMarkScreen;
