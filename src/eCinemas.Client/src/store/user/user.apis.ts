import {
  IGetMeResponse,
  ISignInRequest,
  ISignInResponse,
  ISignUpRequest,
} from "~/store/user/user.interfaces";
import { client, setClientAccessToken } from "~/core/client";
import { IAPIResponse } from "~/core/interfaces";

const endpoint = {
  signIn: "/api/user/sign-in",
  signUp: "/api/user/sign-up",
  me: "/api/user/me",
};

export const signInAPI = async (payload: ISignInRequest) =>
  await client.post<IAPIResponse<ISignInResponse>>(endpoint.signIn, payload);

export const signUpAPI = async (payload: ISignUpRequest) =>
  await client.post<IAPIResponse<any>>(endpoint.signUp, payload);

export const getMeAPI = async () =>
  await client.get<IAPIResponse<IGetMeResponse>>(endpoint.me);
