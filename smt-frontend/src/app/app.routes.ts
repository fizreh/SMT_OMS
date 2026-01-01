import { Routes } from '@angular/router';
import { authGuard } from './core/auth/auth.guard';
import { Login } from './features/auth/login/login';
import { MainScreen } from './features/main-screen/main-screen';

export const APP_ROUTES: Routes = [
  { path: '', 
    component: MainScreen, 
    canActivate: [authGuard] 
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
  },
  { 
    path: '**', 
    redirectTo: '',
  }  
];



