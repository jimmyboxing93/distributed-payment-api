import { createReducer, on, State } from '@ngrx/store';
import { AuthActions, AuthApiActions, UserProfile } from './auth.actions';


export interface AuthState
{
  user: UserProfile | null,
  token: string | null,
  isLoading: boolean,
  error: string | null,
  IsSideBarOpen: boolean;
}

export const initialState: AuthState =
{
  user: null,
  token: null,
  isLoading: false,
  error: null,
  IsSideBarOpen: true
};

export const authReducer = createReducer(
  initialState,

  on(AuthActions.loginRequest, (state) => (
    {
      ...state,
      isLoading: true,
      error: null
    })),
  // Handle successful API responses
  on(AuthApiActions.loginSuccess, (state, { user, token }) => (
    {
      ...state,
      user,
      token,
      isLoading: false,
      error: null
    })),
  // Handle errors gracefully without blowing up the shell
  on(AuthApiActions.loginFailure, (state, { error }) => (
    {
      ...state,
      user: null,
      token: null,
      isLoading: false,
      error
    })),

  // Clear out tokens on logout
  on(AuthActions.logoutRequest, () => (
    {
      ...initialState,
      IsSideBarOpen: false
    })),

  on(AuthActions.toggleSidebar, (state) => (
    {
      ...state,
      IsSideBarOpen: !state.IsSideBarOpen
    }))
);
