import React, { useEffect, useState } from "react";
import {
  FlatList,
  View,
  Image,
  Text,
  StyleSheet,
  TouchableOpacity,
} from "react-native";
import { useAppDispatch, useAppSelector } from "~/features/store";
import { colors } from "~/shared/constants";
import { EMovieStatus, IMovieViewModel, listMovie } from "~/features/movie";
import { router } from "expo-router";

const HomeScreen = () => {
  const [status, setStatus] = useState<EMovieStatus>(EMovieStatus.NowShowing);
  const [pageIndex, setPageIndex] = useState<number>(1);
  const pageSize = 10;
  const dispatch = useAppDispatch();
  const featureMovieSelector = useAppSelector((state) => state.movie);

  const loadData = () => {
    dispatch(listMovie({ pageIndex, pageSize, status }));
  };

  useEffect(() => {
    loadData();
  }, [status, pageIndex]);

  return (
    <View style={{ backgroundColor: colors.dark, flex: 1 }}>
      <MovieStatusComponent currentStatus={status} setStatus={setStatus} />
      <FlatList
        data={featureMovieSelector.list}
        keyExtractor={(item) => item.id}
        renderItem={({ item }) => <MovieComponent item={item} />}
        numColumns={2}
        onRefresh={() => loadData()}
        refreshing={false}
        ListEmptyComponent={() => <Text>Không có dữ liệu</Text>}
      />
    </View>
  );
};

const MovieComponent = (props: { item: IMovieViewModel }) => {
  const { item } = props;
  return (
    <TouchableOpacity
      className="flex-[0.5]"
      onPress={() =>
        router.push({
          pathname: "/(main)/home/movie-detail",
          params: { id: item.id },
        })
      }
    >
      <Image source={{ uri: item.posterUrl }} className="h-[300px]" />
      <Text className="text-white">{item.title}</Text>
    </TouchableOpacity>
  );
};

const MovieStatusComponent = (props: {
  currentStatus: EMovieStatus;
  setStatus: (status: EMovieStatus) => void;
}) => {
  const { currentStatus, setStatus } = props;
  return (
    <View className="flex-row justify-center mt-2">
      <Text
        onPress={() => setStatus(EMovieStatus.NowShowing)}
        style={{
          ...styles.textStatus,
          color:
            currentStatus === EMovieStatus.NowShowing
              ? colors.primary
              : "white",
        }}
      >
        Now Showing
      </Text>
      <Text
        onPress={() => setStatus(EMovieStatus.ComingSoon)}
        style={{
          ...styles.textStatus,
          color:
            currentStatus === EMovieStatus.ComingSoon
              ? colors.primary
              : "white",
        }}
      >
        Comming Soon
      </Text>
    </View>
  );
};

const styles = StyleSheet.create({
  textStatus: {
    color: colors.primary,
    fontSize: 18,
    flex: 1,
    textAlign: "center",
    padding: 12,
  },
});

export default HomeScreen;
