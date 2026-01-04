import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../../core/auth/auth.service';
import { Router } from '@angular/router';
import { MATERIAL_PROVIDERS } from '../../../shared/material/material.providers';

@Component({
  standalone: true,
  imports: [CommonModule, FormsModule,
    MATERIAL_PROVIDERS
  ],
  templateUrl: './login.html',
  styleUrl: './login.css'
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
      this.router.navigate(['/']);
    } catch (e: any) {
  if (e && e.message) {
    this.error = e.message;
  } else {
    this.error = 'Login failed';
  }
}
  }
}