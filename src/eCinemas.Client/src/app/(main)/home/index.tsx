import { router } from "expo-router";
import React, { useEffect, useState } from "react";
import {
  FlatList,
  Image,
  StyleSheet,
  Text,
  TouchableOpacity,
  View,
} from "react-native";
import {
  EMovieStatus,
  IMovieViewModel,
  listMovie,
  selectMovie,
} from "~/features/movie";
import { useAppDispatch, useAppSelector } from "~/features/store";
import { colors } from "~/shared/constants";

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
    <View style={{ backgroundColor: colors.dark, flex: 1 }} className="px-1">
      <MovieStatusComponent currentStatus={status} setStatus={setStatus} />
      <FlatList
        data={featureMovieSelector.list}
        keyExtractor={(item) => item.id}
        renderItem={({ item }) => <MovieComponent item={item} />}
        numColumns={3}
        onRefresh={() => loadData()}
        refreshing={false}
        ListEmptyComponent={() => <Text>Không có dữ liệu</Text>}
      />
    </View>
  );
};

const MovieComponent = (props: { item: IMovieViewModel }) => {
  const { item } = props;
  const dispatch = useAppDispatch();
  return (
    <View className="flex-[0.3333] p-1">
      <TouchableOpacity
        onPress={() => {
          router.push({
            pathname: "/(main)/home/movie-detail",
            params: { id: item.id },
          });
          dispatch(selectMovie(item.id));
        }}
      >
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
  return (
    <View className="flex-row justify-center mt-2">
      <Text
        onPress={() => setStatus(EMovieStatus.NowShowing)}
        style={{
          ...styles.textStatus,
          fontFamily: "LexendDeca",
          fontSize: 15,
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
          fontFamily: "LexendDeca",
          fontSize: 15,
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
