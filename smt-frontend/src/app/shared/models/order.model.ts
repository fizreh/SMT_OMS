import { OrderBoard } from "./order-board.model";

export interface Order {
  id: string;
  name: string;
  description: string;
  orderDate: string;
  boards: OrderBoard[];
}