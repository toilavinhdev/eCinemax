import { client } from "~/core/client";
import {
  IListNotificationRequest,
  IListNotificationResponse,
} from "./notification.interfaces";
import { IAPIResponse } from "~/core/interfaces";

const endpoints = {
  list: "/api/notification/list",
};

export const listNotificationAPI = (payload: IListNotificationRequest) =>
  client.request<IAPIResponse<IListNotificationResponse>>({
    method: "POST",
    url: endpoints.list,
    data: payload,
  });
