import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OrdersService } from '../../services/orders.service';
import { Order } from '../../../../shared/models/order.model';
import { OrderDetailComponent } from "../order-detail/order-detail";
import { Router } from '@angular/router';

@Component({
  standalone: true,
  selector: 'app-orders-list',
  imports: [CommonModule],
  templateUrl: './order-list.html'
})
export class OrdersListComponent implements OnInit {
  selectedOrderId: string | null = null;
  orders: Order[] = [];
  loadingId: string | null = null;
  message = '';
  successId: string | null = null;
 

  constructor(private ordersService: OrdersService, private router: Router) {}

  ngOnInit() {
    this.ordersService.getOrders().subscribe({
      next: orders => (this.orders = orders),
      error: () => (this.message = 'Failed to load orders')
    });
  }
   viewOrder(orderId: string) {
    this.router.navigate(['/orders', orderId]);
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