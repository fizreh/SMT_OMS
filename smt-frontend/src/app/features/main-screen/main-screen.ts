import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-main-screen',
  standalone: true,
  templateUrl: './main-screen.html',
  styles: [`
    .actions { display: flex; justify-content: center; gap: 20px; margin-top: 50px; }
    button { padding: 10px 20px; font-size: 16px; }
  `]
})
export class MainScreen {
  constructor(private router: Router) {}

  goToCreateOrder() {
    this.router.navigate(['/orders/create']);
  }

  goToViewOrders() {
    this.router.navigate(['/orders']);
  }
}