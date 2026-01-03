import { BoardOrderModel } from "./order-board-create-model";

export interface CreateOrderModel {
  name: string;
  description: string;
  orderDate: string; // ISO string
  boards: BoardOrderModel[];
}