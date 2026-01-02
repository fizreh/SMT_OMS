import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OrdersService } from '../../services/orders.service';
import { Order } from '../../../../shared/models/order.model';
import { OrderDetailComponent } from "../order-detail/order-detail";
import { Router } from '@angular/router';
import { MatIcon } from "@angular/material/icon";
import { MatDivider } from "@angular/material/divider";
import { MATERIAL_PROVIDERS } from '../../../../shared/material/material.providers';

@Component({
  standalone: true,
  selector: 'app-orders-list',
  imports: [CommonModule, MATERIAL_PROVIDERS,],
  templateUrl: './order-list.html',
  styleUrl: './order-list.css'
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

confirmDelete(orderId: string) {
  const confirmed = confirm('Are you sure you want to delete this order?');
  if (!confirmed) return;

  this.ordersService.deleteOrder(orderId).subscribe({
    next: () => {
      this.orders = this.orders.filter(o => o.id !== orderId);
    },
    error: (err) => {
      console.error('Delete failed', err);
    }
  });
}
}