import { View, Text, FlatList, Image } from "react-native";
import React, { useEffect, useState } from "react";
import { colors } from "~/shared/constants";
import { NoDataComponent, SpinnerFooterComponent } from "~/shared/components";
import { useAppDispatch, useAppSelector } from "~/features/store";
import {
  INotificationViewModel,
  listNotification,
} from "~/features/notification";
import moment from "moment";
import { hideGlobalLoading, showGlobalLoading } from "~/features/common";

const NotificationScreen = () => {
  const dispatch = useAppDispatch();
  const { list, pagination, loading, error } = useAppSelector(
    (state) => state.notification
  );
  const [pageIndex, setPageIndex] = useState<number>(1);
  const PAGE_SIZE = 10;

  const loadData = (idx: number) => {
    dispatch(
      listNotification({
        pageIndex: idx,
        pageSize: PAGE_SIZE,
      })
    );
  };

  const getNextBatch = () => {
    if (loading || !pagination?.hasNextPage) return;
    loadData(pageIndex + 1);
    setPageIndex(pageIndex + 1);
  };

  const refresh = () => {
    setPageIndex(1);
    loadData(1);
  };

  useEffect(() => {
    refresh();
  }, []);

  useEffect(() => {
    if (loading) {
      dispatch(showGlobalLoading());
    } else {
      dispatch(hideGlobalLoading());
    }
  }, [loading]);

  if (list.length === 0) {
    return (
      <View className="flex-1" style={{ backgroundColor: colors.dark }}>
        <NoDataComponent />
      </View>
    );
  }

  return (
    <View className="flex-1 p-1" style={{ backgroundColor: colors.dark }}>
      <FlatList
        data={list}
        keyExtractor={(item) => item.id}
        renderItem={({ item }) => <NotificationComponent data={item} />}
        numColumns={1}
        onRefresh={refresh}
        refreshing={false}
        onEndReachedThreshold={0}
        onEndReached={getNextBatch}
        showsVerticalScrollIndicator={false}
        ListEmptyComponent={() => <NoDataComponent />}
        ItemSeparatorComponent={() => (
          <View className="h-px bg-gray-700 my-2" />
        )}
        ListFooterComponent={() => (
          <SpinnerFooterComponent show={pagination?.hasNextPage ?? false} />
        )}
      />
    </View>
  );
};

const NotificationComponent = (props: { data: INotificationViewModel }) => {
  const { data } = props;

  return (
    <View className="flex-row gap-x-3">
      <Image
        source={{ uri: data.photoUrl }}
        className="h-[90px] w-[120px] rounded-lg"
      />
      <View className="flex-1 justify-between">
        <Text className="text-white text-[12px]">{data.title}</Text>
        <Text className="text-gray-400 text-[10px]">
          {moment(data.createdAt).format("HH:mm DD/MM/yyyy")}
        </Text>
      </View>
    </View>
  );
};

export default NotificationScreen;
