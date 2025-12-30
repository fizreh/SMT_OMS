import { Injectable } from '@angular/core';
import { ApiService } from '../../../core/services/api.service';
import { Order } from '../../../shared/models/order.model';

@Injectable()
export class OrdersService {
  constructor(private api: ApiService) {}

  getOrders() {
    return this.api.get<Order[]>('orders');
  }
}