import { bootstrapApplication } from '@angular/platform-browser';
import { provideRouter } from '@angular/router';
import { AppComponent } from './app/app';
import { APP_ROUTES } from './app/app.routes';
import { CORE_PROVIDERS } from './app/core/providers';

bootstrapApplication(AppComponent, {
  providers: [
    provideRouter(APP_ROUTES),
    ...CORE_PROVIDERS
  ]
}).catch(err => console.error(err));