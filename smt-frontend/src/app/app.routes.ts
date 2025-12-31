import { Routes } from '@angular/router';

export const APP_ROUTES: Routes = [
  {
    path: 'orders',
    loadChildren: () =>
      import('./features/orders/orders.routes')
        .then(m => m.ORDERS_ROUTES)
  },
  {
    path: '',
    redirectTo: 'orders',
    pathMatch: 'full'
  }
];

