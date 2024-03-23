export interface IUserState {
  isAuthorized: boolean;
  loading?: boolean;
  currentUser?: IGetMeResponse;
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
