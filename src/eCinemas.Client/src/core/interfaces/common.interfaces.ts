export interface IAPIResponse<T> {
  success: false;
  code: number;
  message?: string;
  errors?: string[];
  data: T;
}

export interface IPagination {
  pageIndex: number;
  pageSize: number;
  totalRecord: number;
  totalPage: number;
  hasPreviosPage: number;
  hasNextPpage: number;
}

export interface IPaginationResponse<T> {
  pagination: IPagination;
  records: T[];
}
