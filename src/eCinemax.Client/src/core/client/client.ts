import axios, { AxiosError, AxiosResponse } from "axios";
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
  (response: AxiosResponse) => {
    const timeLog = new Date(Date.now());
    const { url, params, data } = response.config;
    console.log(
      `[${timeLog.getHours()}:${timeLog.getMinutes()}:${timeLog.getMilliseconds()}]`,
      "API RESPONSE",
      url,
      JSON.stringify(params),
      JSON.stringify(data)
    );
    return response;
  },
  (error: AxiosError) => {
    const timeLog = new Date(Date.now());
    const response = error.response!.data as IAPIResponse<any>;
    const { code, errors } = response;
    console.log(
      `[${timeLog.getHours()}:${timeLog.getMinutes()}:${timeLog.getMilliseconds()}]`,
      "API ERROR",
      code,
      error.config?.url,
      JSON.stringify(errors)
    );
    throw new Error(errors?.[0]);
  }
);
