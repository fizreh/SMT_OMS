import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OrdersService } from '../../services/orders.service';
import { Order } from '../../../../shared/models/order.model';
import { OrderDetailComponent } from "../order-detail/order-detail";

@Component({
  standalone: true,
  selector: 'app-orders-list',
  imports: [CommonModule, OrderDetailComponent],
  providers: [OrdersService],
  template: `
    <h2>Orders</h2>

    <ul>
      <li *ngFor="let order of orders">
        {{ order.name }}  {{ order.orderDate }}

          <button
          (click)="selectedOrderId = order.id" 
          [disabled]="loadingId === order.id"
        >
           {{ loadingId === order.id ? 'Downloading...' : 'Download to Production' }}
        </button>
        <app-order-detail *ngIf="selectedOrderId" [orderId]="selectedOrderId"></app-order-detail>
         <span *ngIf="successId === order.id" style="color: green;">
      ✓ Downloaded
    </span>
      </li>
    </ul>
    
  `
})
export class OrdersListComponent implements OnInit {
  selectedOrderId: string | null = null;
  orders: Order[] = [];
  loadingId: string | null = null;
  message = '';
  successId: string | null = null;

  constructor(private ordersService: OrdersService) {}

  ngOnInit() {
    this.ordersService.getOrders().subscribe({
      next: orders => (this.orders = orders),
      error: () => (this.message = 'Failed to load orders')
    });
  }

//   download(orderId: string) {
//   this.loadingId = orderId;
//   this.successId = null;
//   this.message = '';

//   this.ordersService.downloadOrder(orderId).subscribe({
//     next: () => {
//       this.successId = orderId;
//       this.loadingId = null;
//       this.message = `Order ${orderId} sent to production line`;
//     },
//     error: () => {
//       this.loadingId = null;
//       this.message = `Failed to download order ${orderId}`;
//     }
//   });
// }
}