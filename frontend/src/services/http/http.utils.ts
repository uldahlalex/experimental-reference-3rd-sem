import {AxiosRequestConfig} from "axios";

export function withJwt(config?: AxiosRequestConfig | undefined): AxiosRequestConfig {
  return {
    ...config,
    headers: {
      ...(config?.headers ?? {}),
      Authorization: localStorage.getItem('jwt')
    }
  };
}
