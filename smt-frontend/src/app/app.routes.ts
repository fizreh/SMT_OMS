import { Routes } from '@angular/router';

export const APP_ROUTES: Routes = [
  {
    path: 'orders',
    loadChildren: () =>
      import('./features/orders/orders-routes')
        .then(m => m.OrdersRoutes)
  },
  {
    path: '',
    redirectTo: 'orders',
    pathMatch: 'full'
  }
];