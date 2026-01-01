import { Routes } from '@angular/router';

export const ORDERS_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./pages/order-list/order-list')
        .then(m => m.OrdersListComponent)
  },
  {
    path: 'create',
    loadComponent: () =>
      import('./pages/order-create/order-create')
        .then(m => m.CreateOrderComponent)
  },
  {
    path: ':id',
    loadComponent: () =>
      import('./pages/order-detail/order-detail')
        .then(m => m.OrderDetailComponent)
  },
  
  
];

