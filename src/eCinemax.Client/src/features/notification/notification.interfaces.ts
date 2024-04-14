import { IPagination, IPaginationResponse } from "~/core/interfaces";

export interface INotificationState {
  loading: boolean;
  error: string | null;
  list: INotificationViewModel[];
  pagination: IPagination | null;
}

export interface IListNotificationRequest {
  pageIndex: number;
  pageSize: number;
}

export interface IListNotificationResponse
  extends IPaginationResponse<INotificationViewModel> {}

export interface INotificationViewModel {
  id: string;
  title: string;
  content: string;
  photoUrl: string;
  createdAt: Date;
}
