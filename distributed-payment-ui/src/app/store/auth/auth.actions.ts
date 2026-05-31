import { createActionGroup, emptyProps, props } from '@ngrx/store'

export interface UserProfile
{
  id: string;
  email: string;
  role: string[];
}

export const AuthActions = createActionGroup
  ({
    source: 'AuthPage',
    events:
    {
      'Login Request': props <{ username: string; password: string }>(),
      'Logout Request': emptyProps(),
      'Toggle Sidebar': emptyProps(),
    }
  });


export const AuthApiActions = createActionGroup
  ({
    source: 'Auth Api',
    events:
    {
      'Login Success': props<{ user: UserProfile; token: string }>(),
      'Login failure': props<{ error: string }>(),
    }
  });


