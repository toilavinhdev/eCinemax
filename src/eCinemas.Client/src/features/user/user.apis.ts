import { client } from "~/core/client";
import { IAPIResponse } from "~/core/interfaces";
import {
  IGetMeResponse,
  ISignInRequest,
  ISignInResponse,
  ISignUpRequest,
} from "~/features/user/user.interfaces";

const endpoints = {
  signIn: "/api/user/sign-in",
  signUp: "/api/user/sign-up",
  me: "/api/user/me",
};

export const signInAPI = async (payload: ISignInRequest) =>
  await client.request<IAPIResponse<ISignInResponse>>({
    method: "POST",
    data: payload,
    url: endpoints.signIn,
  });

export const signUpAPI = async (payload: ISignUpRequest) =>
  await client.request<IAPIResponse<any>>({
    method: "POST",
    data: payload,
    url: endpoints.signUp,
  });

export const getMeAPI = async () =>
  await client.request<IAPIResponse<IGetMeResponse>>({
    method: "POST",
    url: endpoints.me,
  });
