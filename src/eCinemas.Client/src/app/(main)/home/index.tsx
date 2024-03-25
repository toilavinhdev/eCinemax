import React, { useEffect, useState } from "react";
import { FlatList, View } from "react-native";
import { useAppDispatch, useAppSelector } from "~/features/store";
import { colors } from "~/shared/constants";
import { MovieComponent, MovieStatusComponent } from "~/shared/components";
import { EMovieStatus, listMovie } from "~/features/movie";

const HomeScreen = () => {
  const [status, setStatus] = useState<EMovieStatus>(EMovieStatus.NowShowing);
  const [pageIndex, setPageIndex] = useState<number>(1);
  const pageSize = 10;
  const dispatch = useAppDispatch();
  const list = useAppSelector((state) => state.movie.list);

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
        data={list}
        renderItem={({ item }) => <MovieComponent movie={item} />}
      />
    </View>
  );
};

export default HomeScreen;
