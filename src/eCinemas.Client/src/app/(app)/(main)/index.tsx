import { router } from "expo-router";
import React, { useEffect, useState } from "react";
import { FlatList, Image, Text, TouchableOpacity, View } from "react-native";
import { EMovieStatus, IMovieViewModel, listMovie } from "~/features/movie";
import { useAppDispatch, useAppSelector } from "~/features/store";
import { NoDataComponent } from "~/shared/components";
import { colors } from "~/shared/constants";

const HomeScreen = () => {
  const [status, setStatus] = useState<EMovieStatus>(EMovieStatus.NowShowing);
  const [pageIndex, setPageIndex] = useState<number>(1);
  const pageSize = 15;
  const dispatch = useAppDispatch();
  const featureMovieSelector = useAppSelector((state) => state.movie);

  const loadData = () => {
    dispatch(listMovie({ pageIndex, pageSize, status }));
  };

  useEffect(() => {
    loadData();
  }, [status, pageIndex]);

  return (
    <View style={{ backgroundColor: colors.dark }} className="flex-1 px-1">
      <MovieStatusComponent currentStatus={status} setStatus={setStatus} />
      <FlatList
        data={featureMovieSelector.list}
        keyExtractor={(item) => item.id}
        renderItem={({ item }) => <MovieComponent item={item} />}
        numColumns={3}
        onRefresh={() => loadData()}
        refreshing={false}
        ListEmptyComponent={() => <NoDataComponent />}
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
