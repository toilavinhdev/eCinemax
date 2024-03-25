import axios from "axios";
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
      JSON.stringify(response.config.url),
      JSON.stringify(response.config.params),
      JSON.stringify(response.config.data),
      response.config.url,
      JSON.stringify(response.data)
    );
    return response;
  },
  (error) => {
    const timeLog = new Date(Date.now());
    const errorResponse: IAPIResponse<any> = error.response.data;
    console.log(
      `[${timeLog.getHours()}:${timeLog.getMinutes()}:${timeLog.getMilliseconds()}]`,
      "API ERROR",
      JSON.stringify(errorResponse)
    );
    throw new Error(errorResponse.errors?.[0]);
  }
);
