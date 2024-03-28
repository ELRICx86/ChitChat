import { identity } from "./Identity";

export interface LoginResponse {
    statusCode: string;
    status: boolean;
    message: string;
    identity: identity
  }