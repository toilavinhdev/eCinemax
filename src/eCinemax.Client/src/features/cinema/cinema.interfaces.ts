export interface ICinemaState {
  loading: boolean;
  cinemas: ICinemaViewModel[];
}

export interface ICinemaViewModel {
  id: string;
  name: string;
  address: string;
  location?: ICinemaLocationViewModel;
}

export interface ICinemaLocationViewModel {
  latitude: number;
  longitude: number;
}
