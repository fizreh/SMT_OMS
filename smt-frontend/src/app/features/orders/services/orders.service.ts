import { Injectable } from '@angular/core';
import { ApiService } from '../../../core/services/api.service';
import { Order } from '../../../shared/models/order.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class OrdersService {
  constructor(private api: ApiService) {}

   createOrder(order:  any): Observable<any> {
    return this.api.post<any>(`orders`, order);
  }
   addBoardToOrder(orderId: string, boardId: string): Observable<any> {
    return this.api.post<any>(`orders/${orderId}/boards`, boardId);
  }
  getOrders() {
    return this.api.get<Order[]>('orders');
  }

  downloadOrder(orderId: string): Observable<any> {
    return this.api.post<any>(`orders/${orderId}/download`, {});
  }
  
}