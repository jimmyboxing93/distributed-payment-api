import { createFeatureSelector, createSelector } from '@ngrx/store';
import { ChatState } from './chat.reducer';

export const selectChatState = createFeatureSelector<ChatState>('chat');

export const selectChatMessages = createSelector(
  selectChatState,
  (state) => state.message
);

export const selectChatLoading = createSelector(
  selectChatState,
  (state) => !state.isLoading
);

export const selectChatError = createSelector(
  selectChatState,
  (state) => state.error
);
