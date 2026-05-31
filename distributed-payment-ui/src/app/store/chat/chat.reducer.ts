import { createReducer, on, State } from '@ngrx/store';
import { ChatActions, ChatApiActions, ChatMessage } from './chat.actions';


export interface ChatState {
  message: ChatMessage[];
  isLoading: boolean,
  error: string | null,
}

export const initialState: ChatState =
{
  message: [],
  isLoading: false,
  error: null,
};

export const chatReducer = createReducer(
  initialState,

  on(ChatActions.loadMessages, (state) => (
    {
      ...state,
      isLoading: true,
      error: null
    })),
  // Handle successful API responses
  on(ChatApiActions.loadMessageSuccess, (state, { message }) => (
    {
      ...state,
      message,
      isLoading: false,
      error: null
    })),
  // Handle errors gracefully without blowing up the shell
  on(ChatApiActions.loadMessageFailure, (state, { error }) => (
    {
      ...state,
      isLoading: false
    }))
  );
