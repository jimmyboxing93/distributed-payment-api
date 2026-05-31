import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideStore } from '@ngrx/store';

import { routes } from './app.routes';
import { authReducer } from './store/auth/auth.reducer';
import { chatReducer } from './store/chat/chat.reducer';

export const appConfig: ApplicationConfig = {
  providers: [provideZoneChangeDetection({ eventCoalescing: true }), provideRouter(routes),

    provideStore
    ({
      auth: authReducer,
      chat: chatReducer
    })
  ]
};
