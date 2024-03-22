import axios from "axios";
import AsyncStorage from "@react-native-async-storage/async-storage";
import { authConst } from "~/shared/constants";

export const client = axios.create({
  baseURL: process.env.EXPO_PUBLIC_BASE_URL,
  timeout: 10000,
});

client.interceptors.request.use(async (config) => {
  const accessToken = await AsyncStorage.getItem(authConst.ACCESS_TOKEN);
  config.headers.Authorization = `Bearer ${accessToken}`;
  config.headers.Accept = "application/json";
  return config;
});
