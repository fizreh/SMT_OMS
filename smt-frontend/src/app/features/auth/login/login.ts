import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../../core/auth/auth.service';
import { Router } from '@angular/router';

@Component({
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <h2>Login</h2>

    <input [(ngModel)]="email" placeholder="Email" />
    <input [(ngModel)]="password" type="password" placeholder="Password" />

    <button (click)="login()">Login</button>

    <p *ngIf="error">{{ error }}</p>
  `
})
export class Login {
  email = '';
  password = '';
  error = '';

  constructor(
    private auth: AuthService,
    private router: Router
  ) {}

  async login() {
    try {
      await this.auth.login(this.email, this.password);
      this.router.navigate(['/orders']);
    } catch (e) {
      this.error = 'Login failed';
    }
  }
}