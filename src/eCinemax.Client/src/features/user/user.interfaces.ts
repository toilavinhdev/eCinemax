export interface IUserState {
  status: "idle" | "loading" | "success" | "failed";
  error: string | null;
  signedIn: boolean;
  currentUser: IGetMeResponse | null;
}

export interface IUserClaimValue {
  exp: number;
  id: string;
  fullName: string;
  email: string;
}

export interface ISignInRequest {
  email: string;
  password: string;
}

export interface ISignInResponse {
  accessToken: string;
}

export interface ISignUpRequest {
  fullName: string;
  email: string;
  password: string;
}

export interface IGetMeResponse {
  id: string;
  fullName: string;
  email: string;
  avatarUrl?: string;
}

export interface IUpdatePasswordRequest {
  email: string;
  currentPassword: string;
  newPassword: string;
}

export interface IUpdateProfileRequest {
  fullName: string;
  email: string;
  avatarUrl?: string;
}
