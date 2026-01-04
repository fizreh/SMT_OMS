import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { MATERIAL_PROVIDERS } from '../../shared/material/material.providers';
import { AuthService } from '../../core/auth/auth.service';

@Component({
  selector: 'app-main-screen',
  standalone: true,
  imports: [
    MATERIAL_PROVIDERS,
    ],
  templateUrl: './main-screen.html',
  styleUrl:'./main-screen.css'
})
export class MainScreen {
  constructor(private router: Router, private auth:AuthService) {}

  goToCreateOrder() {
    this.router.navigate(['/orders/create']);
  }

  goToViewOrders() {
    this.router.navigate(['/orders']);
  }

   onSignOut() {
    this.auth.logout().then(() => {
      console.log('User signed out');
      this.router.navigate(['/login']); // redirect to login page
    }).catch((err) => {
      console.error('Sign out error:', err);
    });
  }
  
}