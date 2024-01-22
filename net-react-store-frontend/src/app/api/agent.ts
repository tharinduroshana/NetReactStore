import axios, { AxiosError, AxiosResponse } from "axios";
import { toast } from "react-toastify";
import { routes } from "../router/Routes";
import { PaginatedResponse } from "../models/pagination";

axios.defaults.baseURL = "http://localhost:5198/api";
axios.defaults.withCredentials = true;

const responseBody = (response: AxiosResponse) => response?.data;

axios.interceptors.response.use(
  async (response) => {
    const pagination = response.headers["pagination"];
    if (pagination) {
      response.data = new PaginatedResponse(
        response.data,
        JSON.parse(pagination),
      );
      return response;
    }
    return response;
  },
  (error: AxiosError) => {
    const { data, status } = error.response as AxiosResponse;
    switch (status) {
      case 400: {
        if (data.errors) {
          const modelStateErrors: string[] = [];
          for (const key in data.errors) {
            if (data.errors[key]) {
              modelStateErrors.push(data.errors[key]);
            }
          }
          throw modelStateErrors.flat();
        }
        toast.error(data.title);
        break;
      }
      case 401:
      case 500: {
        routes.navigate("/server-error", { state: { error: data } });
        break;
      }
      default:
        break;
    }
    return Promise.reject(error.response);
  },
);

const requests = {
  get: (url: string, params?: URLSearchParams) =>
    axios.get(url, { params }).then(responseBody),
  post: (url: string, body: {}) => axios.post(url, body).then(responseBody),
  put: (url: string, body: {}) => axios.put(url, body).then(responseBody),
  delete: (url: string) => axios.delete(url).then(responseBody),
};

const Catalog = {
  list: (params: URLSearchParams) => requests.get("Products", params),
  fetchFilters: () => requests.get("products/filters"),
  details: (id: string) => requests.get(`products/${id}`),
};

const TestError = {
  get400Error: () => requests.get("buggy/bad-request"),
  get401Error: () => requests.get("buggy/unauthorized"),
  get404Error: () => requests.get("buggy/not-found"),
  get500Error: () => requests.get("buggy/server-error"),
  getValidationError: () => requests.get("buggy/validation-error"),
};

const Basket = {
  get: () => requests.get("basket"),
  addItem: (productId: number, quantity = 1) =>
    requests.post(`basket?productId=${productId}&quantity=${quantity}`, {}),
  removeItem: (productId: number, quantity = 1) =>
    requests.delete(`basket?productId=${productId}&quantity=${quantity}`),
};

const Account = {
  login: (values: any) => requests.post("/User/login", values),
  register: (values: any) => requests.post("/User/register", values),
};

const agent = {
  Catalog,
  TestError,
  Basket,
  Account,
};

export default agent;
