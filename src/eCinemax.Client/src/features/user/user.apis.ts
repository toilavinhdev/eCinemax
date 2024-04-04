import { client } from "~/core/client";
import { IAPIResponse } from "~/core/interfaces";
import {
  IGetMeResponse,
  ISignInRequest,
  ISignInResponse,
  ISignUpRequest,
  IUpdatePasswordRequest,
  IUpdateProfileRequest,
} from "~/features/user/user.interfaces";

const endpoints = {
  signIn: "/api/user/sign-in",
  signUp: "/api/user/sign-up",
  me: "/api/user/me",
  updatePassword: "/api/user/update-password",
  updateProfile: "/api/user/update-profile",
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

export const updatePasswordAPI = async (payload: IUpdatePasswordRequest) =>
  await client.request<IAPIResponse<any>>({
    method: "PUT",
    url: endpoints.updatePassword,
    data: payload,
  });

export const updateProfileAPI = async (payload: IUpdateProfileRequest) =>
  await client.request<IAPIResponse<IGetMeResponse>>({
    method: "PUT",
    url: endpoints.updateProfile,
    data: payload,
  });
