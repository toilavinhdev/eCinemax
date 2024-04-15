import AsyncStorage from "@react-native-async-storage/async-storage";
import axios, {
  AxiosError,
  AxiosResponse,
  InternalAxiosRequestConfig,
} from "axios";
import moment from "moment";
import { IAPIResponse } from "~/core/interfaces";
import { authConst } from "~/shared/constants";

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
  logRequest(config);
  return config;
});

client.interceptors.response.use(
  (response: AxiosResponse) => {
    logResponse(response);
    return response;
  },
  (error: AxiosError) => {
    const response = error.response!.data as IAPIResponse<any>;
    const { errors } = response;
    logError(error);
    throw new Error(errors?.[0]);
  }
);

/** Logging **/

const logRequest = (config: InternalAxiosRequestConfig<any> | undefined) => {
  console.log(getTimeLog(), "REQUEST", getUrl(config));
};

const logResponse = (response: AxiosResponse<any, any>) => {
  const { params, data } = response.config;
  console.log(
    getTimeLog(),
    "RESPONSE",
    getUrl(response.config),
    JSON.stringify(params),
    JSON.stringify(data)
  );
};

const logError = (error: AxiosError<unknown, any>) => {
  const response = error.response!.data as IAPIResponse<any>;
  const { code, errors } = response;
  console.log(
    getTimeLog(),
    "ERROR",
    getUrl(error.config),
    code,
    JSON.stringify(errors)
  );
};

const getTimeLog = (): string =>
  "[" + moment(new Date(Date.now())).format("HH:mm:ss") + "]";

const getUrl = (
  config: InternalAxiosRequestConfig<any> | undefined
): string => {
  return `${config?.baseURL}${config?.url}`;
};
