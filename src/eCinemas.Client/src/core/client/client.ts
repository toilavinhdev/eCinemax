import axios, { AxiosError } from "axios";
import AsyncStorage from "@react-native-async-storage/async-storage";
import { authConst } from "~/shared/constants";
import { IAPIResponse } from "~/core/interfaces";

export const client = axios.create({
  baseURL: process.env.EXPO_PUBLIC_BASE_URL,
  timeout: 10000,
});

client.interceptors.request.use(async (config) => {
  const accessToken = await AsyncStorage.getItem(authConst.ACCESS_TOKEN);
  if (accessToken) {
    config.headers.Authorization = `Bearer ${accessToken}`;
  }
  config.headers.Accept = "application/json";
  return config;
});

client.interceptors.response.use(
  (response) => {
    const timeLog = new Date(Date.now());
    console.log(
      `[${timeLog.getHours()}:${timeLog.getMinutes()}:${timeLog.getMilliseconds()}]`,
      "API RESPONSE",
      response.config.url,
      JSON.stringify(response.config.params),
      JSON.stringify(response.config.data)
    );
    return response;
  },
  (error: AxiosError) => {
    const timeLog = new Date(Date.now());
    const errorResponse = error.response!.data as IAPIResponse<any>;
    console.log(
      `[${timeLog.getHours()}:${timeLog.getMinutes()}:${timeLog.getMilliseconds()}]`,
      "API ERROR",
      error.config?.url,
      JSON.stringify(errorResponse.errors)
    );
    throw new Error(errorResponse.errors?.[0]);
  }
);
