import { router } from "expo-router";
import React, { useEffect, useState } from "react";
import { FlatList, Image, Text, TouchableOpacity, View } from "react-native";
import { hideGlobalLoading, showGlobalLoading } from "~/features/common";
import { EMovieStatus, IMovieViewModel, listMovie } from "~/features/movie";
import { useAppDispatch, useAppSelector } from "~/features/store";
import { NoDataComponent, SpinnerFooterComponent } from "~/shared/components";
import { colors } from "~/shared/constants";

const HomeScreen = () => {
  const [movieStatusFilter, setMovieStatusFilter] = useState<EMovieStatus>(
    EMovieStatus.NowShowing
  );
  const [pageIndex, setPageIndex] = useState<number>(1);
  const PAGE_SIZE = 12;
  const dispatch = useAppDispatch();
  const { list, pagination, status } = useAppSelector((state) => state.movie);

  const loadData = (idx: number) => {
    dispatch(
      listMovie({
        pageIndex: idx,
        pageSize: PAGE_SIZE,
        status: movieStatusFilter,
      })
    );
  };

  const refresh = () => {
    setPageIndex(1);
    loadData(1);
  };

  const nextBatch = () => {
    if (!pagination?.hasNextPage || status === "loading") return;
    loadData(pageIndex + 1);
    setPageIndex(pageIndex + 1);
  };

  useEffect(() => {
    refresh();
  }, []);

  // Mỗi khi đổi filter trạng thái phim -> refresh (match với ìninite scroll)
  useEffect(() => {
    refresh();
  }, [movieStatusFilter]);

  useEffect(() => {
    if (status === "loading") {
      dispatch(showGlobalLoading());
    } else {
      dispatch(hideGlobalLoading());
    }
  }, [status]);

  return (
    <View style={{ backgroundColor: colors.dark }} className="flex-1 px-1">
      <MovieStatusComponent
        currentStatus={movieStatusFilter}
        setStatus={setMovieStatusFilter}
      />
      <FlatList
        data={list}
        keyExtractor={(item) => item.id}
        renderItem={({ item }) => <MovieComponent item={item} />}
        numColumns={3}
        onRefresh={refresh}
        refreshing={false}
        onEndReachedThreshold={0}
        onEndReached={nextBatch}
        showsVerticalScrollIndicator={false}
        ListEmptyComponent={() => <NoDataComponent />}
        ListFooterComponent={() => (
          <SpinnerFooterComponent show={pagination?.hasNextPage ?? false} />
        )}
      />
    </View>
  );
};

const MovieComponent = (props: { item: IMovieViewModel }) => {
  const { item } = props;

  const navigateToMovieDetail = () => {
    router.push({
      pathname: "/booking/movie-detail",
      params: { id: item.id },
    });
  };

  return (
    <View className="flex-[0.3333] p-1 mt-2">
      <TouchableOpacity onPress={() => navigateToMovieDetail()}>
        <Image
          source={{ uri: item.posterUrl }}
          className="h-[200px] rounded-t-lg"
        />
        <View
          className="rounded-b-lg px-1 py-2"
          style={{ backgroundColor: colors.secondary }}
        >
          <Text
            numberOfLines={1}
            className="text-white text-[12px] text-center"
          >
            {item.title}
          </Text>
        </View>
      </TouchableOpacity>
    </View>
  );
};

const MovieStatusComponent = (props: {
  currentStatus: EMovieStatus;
  setStatus: (status: EMovieStatus) => void;
}) => {
  const { currentStatus, setStatus } = props;

  const Component = (props: { status: EMovieStatus; text: string }) => {
    const { status, text } = props;
    return (
      <TouchableOpacity onPress={() => setStatus(status)} className="flex-1">
        <Text
          style={{
            color: currentStatus === status ? colors.primary : "white",
          }}
          className={`text-center text-[16px] py-6 ${currentStatus === status ? "font-semibold" : ""}`}
        >
          {text}
        </Text>
      </TouchableOpacity>
    );
  };

  return (
    <View className="flex-row items-center">
      <Component status={EMovieStatus.NowShowing} text="Đang chiếu" />
      <Component status={EMovieStatus.ComingSoon} text="Sắp chiếu" />
    </View>
  );
};

export default HomeScreen;
