import axios, { AxiosError, AxiosResponse } from "axios";
import AsyncStorage from "@react-native-async-storage/async-storage";
import { authConst } from "~/shared/constants";
import { IAPIResponse } from "~/core/interfaces";
import { Alert } from "react-native";

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
    console.log(
      `[${timeLog.getHours()}:${timeLog.getMinutes()}:${timeLog.getMilliseconds()}]`,
      "API RESPONSE",
      response.config.url,
      JSON.stringify(response.config.params),
      JSON.stringify(response.config.data)
    );
    const apiResponse = response.data as IAPIResponse<any>;
    if (apiResponse.message) Alert.alert(apiResponse.message);
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
    if (code === 400) Alert.alert(errors?.[0] ?? "");
    // throw new Error(errors?.[0]);
  }
);
