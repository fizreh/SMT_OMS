import { Component, Input, OnInit } from '@angular/core';
import { OrdersService } from '../../services/orders.service';
import { CommonModule, JsonPipe } from '@angular/common';

@Component({
  selector: 'app-order-detail',
  imports: [CommonModule, JsonPipe],
  templateUrl: './order-detail.html',
  styleUrl: './order-detail.css',
})
export class OrderDetailComponent implements OnInit {
  @Input() orderId!: string;   // Receive order ID from parent
  order: any = null;
  loading = false;
  error = '';

  constructor(private ordersService: OrdersService) {}

  ngOnInit(): void {
    if (this.orderId) {
      this.fetchOrder(this.orderId);
    }
  }

  fetchOrder(orderId: string) {
    this.loading = true;
    this.error = '';
    this.ordersService.downloadOrder(orderId).subscribe({
      next: (data:any) => {
        this.order = data;
        this.loading = false;
      },
      error: (err:any) => {
        this.error = 'Failed to fetch order details';
        this.loading = false;
        console.error(err);
      }
    });
  }
}
