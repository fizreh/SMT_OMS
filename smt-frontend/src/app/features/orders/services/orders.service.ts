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
    return this.api.post<any>(`orders/${orderId}/boards`, { boardId });
  }
   addComponentToBoard(orderId: string,boardId: string,componentId: string,quantity: number
  ) {
    return this.api.post<void>(`orders/${orderId}/boards/${boardId}/components`,{ componentId, quantity }
    );
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

