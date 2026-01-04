import { BoardUpdateModel } from "./board-update-model";

export interface OrderUpdateModel {
  id: string;              // Important for identifying the order
  name: string;
  description: string;
  orderDate: string;       // ISO string
  boards: BoardUpdateModel[];
}