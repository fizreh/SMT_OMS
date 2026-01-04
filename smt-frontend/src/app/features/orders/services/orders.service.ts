import { Injectable } from '@angular/core';
import { ApiService } from '../../../core/services/api.service';
import { Order } from '../../../shared/models/order.model';
import { Observable } from 'rxjs';
import { OrderUpdateModel } from '../../../shared/models/update-order-models/order-update.model';

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
 getOrderById(orderId: string) {
  return this.api.get<void>(`orders/${orderId}`);
}

 updateOrder(order: OrderUpdateModel): Observable<void> {
    return this.api.put<void>(`${order}/full`, order);
  }
  downloadOrder(orderId: string): Observable<any> {
    return this.api.post<any>(`orders/${orderId}/download`, {});
  }
  
}

