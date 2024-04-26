import * as SignalR from "@microsoft/signalr";
import AsyncStorage from "@react-native-async-storage/async-storage";
import { authConst } from "~/shared/constants";

export const createWebSocketUrl = (path: string) => {
  const baseSocketUrl = process.env.EXPO_PUBLIC_BASE_WEBSOCKET_URL;
  if (!path.startsWith("/")) path += "/";
  return baseSocketUrl + path;
};

export const createReservationHubConnection = async (showtimeId: string) => {
  const accessToken = await AsyncStorage.getItem(authConst.ACCESS_TOKEN);
  const url = createWebSocketUrl(`/hub/reservation?showTimeId=${showtimeId}`);

  return new SignalR.HubConnectionBuilder()
    .withUrl(url, {
      accessTokenFactory: () => accessToken!,
    })
    .withAutomaticReconnect()
    .build();
};
