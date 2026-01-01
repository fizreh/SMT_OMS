import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { authInterceptor } from './auth/auth.interceptor';

export const CORE_PROVIDERS = [
  provideHttpClient(
    withInterceptors([
      authInterceptor
    ])
  )
];