import { OrderBoard } from './order-board.model';

export interface CreateOrder {
  name: string;
  description: string;
  orderDate: string;   // ISO string (matches your usage)
  boards: OrderBoard[];
}