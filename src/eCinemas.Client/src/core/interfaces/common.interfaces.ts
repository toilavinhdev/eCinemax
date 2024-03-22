export interface IAPIResponse<T> {
  success: false;
  code: number;
  message?: string;
  data: T;
}
