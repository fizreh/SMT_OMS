import { provideHttpClient, withInterceptors } from '@angular/common/http';

export const CORE_PROVIDERS = [
  provideHttpClient(
    withInterceptors([
      // authInterceptor will go here later
    ])
  )
];