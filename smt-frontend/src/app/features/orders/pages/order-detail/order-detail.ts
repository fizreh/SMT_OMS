import { Component, Input, OnInit } from '@angular/core';
import { OrdersService } from '../../services/orders.service';
import { CommonModule, JsonPipe } from '@angular/common';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-order-detail',
  imports: [CommonModule, JsonPipe],
  templateUrl: './order-detail.html',
  styleUrl: './order-detail.css',
})
export class OrderDetailComponent implements OnInit {
  order: any = null;
  loading = false;
  error = '';
  viewMode: 'formatted' | 'json' = 'formatted';

  constructor(private ordersService: OrdersService, private route: ActivatedRoute) {}

  ngOnInit(): void {
    
    const orderId = this.route.snapshot.paramMap.get('id');
    if (!orderId) return;
    
    this.fetchOrder(orderId);

    
  }

   setView(mode: 'formatted' | 'json') {
    this.viewMode = mode;
  }

  fetchOrder(orderId: string) {
    this.loading = true;
    this.error = '';
    this.ordersService.downloadOrder(orderId).subscribe({
      next: (data:any) => {
        this.order = data;
        this.loading = false;
        console.log("Order: ",this.order);//for testing
      },
      error: (err:any) => {
        this.error = 'Failed to fetch order details';
        this.loading = false;
        console.error(err);
      }
    });
    
  }
}
