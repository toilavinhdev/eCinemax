import axios, { AxiosResponse } from "axios";
import AsyncStorage from "@react-native-async-storage/async-storage";
import { authConst } from "~/shared/constants";
import { IAPIResponse } from "../interfaces";

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
    console.log("Client response", response.data);
    return response.data;
  },
  (error) => {
    const errorResponse: IAPIResponse<any> = error.response.data;
    console.log(`Client errors: ${errorResponse.errors}`);
    throw new Error(errorResponse.errors?.[0]);
  }
);
