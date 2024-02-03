import axios, { AxiosError, AxiosResponse } from "axios";
import { toast } from "react-toastify";
import { routes } from "../router/Routes";
import { PaginatedResponse } from "../models/pagination";
import { store } from "../store/configureStore";

axios.defaults.baseURL = "http://localhost:5198/api";
axios.defaults.withCredentials = true;

const responseBody = (response: AxiosResponse) => response?.data;

axios.interceptors.request.use((config: any) => {
  const token = store.getState().account.user?.token;
  if (token) config.headers.Authorization = `Bearer ${token}`;
  return config;
});

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
      case 401: {
        toast.error(data.title);
        break;
      }
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
  get: (username: string | null = null) => {
    if (username !== null) {
      const params = new URLSearchParams();
      params.append("username", username);
      return requests.get("basket", params);
    } else {
      return requests.get("basket");
    }
  },
  addItem: (productId: number, quantity = 1) => {
    const user = localStorage.getItem("user");
    if (user !== null) {
      const username = JSON.parse(user).username;
      return requests.post(
        `basket?productId=${productId}&quantity=${quantity}&username=${username}`,
        {},
      );
    }

    return requests.post(
      `basket?productId=${productId}&quantity=${quantity}`,
      {},
    );
  },
  removeItem: (productId: number, quantity = 1) => {
    const user = localStorage.getItem("user");
    if (user !== null) {
      const username = JSON.parse(user).username;
      return requests.delete(
        `basket?productId=${productId}&quantity=${quantity}&username=${username}`,
      );
    }
    return requests.delete(
      `basket?productId=${productId}&quantity=${quantity}`,
    );
  },
};

const Account = {
  login: (values: any) => requests.post("/User/login", values),
  register: (values: any) => requests.post("/User/signup", values),
  fetchUser: (value: any) => requests.post("/User/fetch-user", value),
  fetchAddress: () => {
    const user = localStorage.getItem("user");
    const username = JSON.parse(user!).username;
    const params = new URLSearchParams();
    params.append("username", username);
    return requests.get("/User/savedAddress", params);
  },
};

const Orders = {
  list: () => {
    const user = localStorage.getItem("user");
    const username = JSON.parse(user!).username;
    const params = new URLSearchParams();
    params.append("username", username);
    return requests.get("/orders", params);
  },
  fetch: (id: string) => {
    const user = localStorage.getItem("user");
    const username = JSON.parse(user!).username;
    const params = new URLSearchParams();
    params.append("username", username);
    return requests.get(`/orders/${id}??username=${username}`);
  },
  create: (values: any) => {
    const user = localStorage.getItem("user");
    const username = JSON.parse(user!).username;
    return requests.post("/orders", { ...values, username: username });
  },
};

const agent = {
  Catalog,
  TestError,
  Basket,
  Account,
  Orders,
};

export default agent;
