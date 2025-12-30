import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OrdersService } from '../../services/orders.service';
import { Order } from '../../../../shared/models/order.model';

@Component({
  standalone: true,
  selector: 'app-orders-list',
  imports: [CommonModule],
  providers: [OrdersService],
  template: `
    <h2>Orders</h2>

    <ul>
      <li *ngFor="let order of orders">
        {{ order.name }} – {{ order.orderDate }}
      </li>
    </ul>
  `
})
export class OrdersListComponent {
  private ordersService = inject(OrdersService);
  orders: Order[] = [];

  ngOnInit(): void {
    this.ordersService.getOrders().subscribe({
      next: data => (this.orders = data),
      error: err => console.error(err)
    });
  }
}