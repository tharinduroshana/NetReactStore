import { Basket } from "./basket";

export type User = {
  username: string;
  name: string;
  token: string;
  basket?: Basket;
};
