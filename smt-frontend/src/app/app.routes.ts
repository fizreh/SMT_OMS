import { Routes } from '@angular/router';
import { authGuard } from './core/auth/auth.guard';
import { Login } from './features/auth/login/login';

export const APP_ROUTES: Routes = [
  {
    path: '',
    redirectTo: 'orders',
    pathMatch: 'full'
  },
  {
    path: 'login',
    component: Login
  },
  {
    path: 'orders',
    canActivate: [authGuard],
    loadChildren: () =>
      import('./features/orders/orders.routes')
        .then(m => m.ORDERS_ROUTES)
  }
  
];

