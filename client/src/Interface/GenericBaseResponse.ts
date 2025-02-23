export interface GenericBaseResponse<T> {
    statusCode: number;
    meta?: object;
    message: string;
    succeeded: boolean;
    errors?: string[];
    data?: T;
  }