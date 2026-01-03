import { Injectable } from '@angular/core';
import { ApiService } from '../../../core/services/api.service';
import { Order } from '../../../shared/models/order.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class OrdersService {
  constructor(private api: ApiService) {}


  createFullOrder(order: any) {
  return this.api.post<any>('orders/full', order);
}
  getOrders() {
    return this.api.get<Order[]>('orders');
  }

  deleteOrder(orderId: string) {
  return this.api.delete<void>(`orders/${orderId}`);
}

  downloadOrder(orderId: string): Observable<any> {
    return this.api.post<any>(`orders/${orderId}/download`, {});
  }
  
}

