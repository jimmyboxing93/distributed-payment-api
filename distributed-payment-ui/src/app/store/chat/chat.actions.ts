import { createAction, createActionGroup, emptyProps, props } from '@ngrx/store';

export interface ChatMessage
{
  id: string;
  senderid: string;
  message: string;
  timestamp: string;
}

export const ChatActions = createActionGroup({
  source: 'Chat Page',
  events:
  {
    'Load Messages': emptyProps(),
    'Send Message': props<{ message: string }>(),

  }
});

export const ChatApiActions = createActionGroup({
  source: 'Chat API',
  events:
  {
    'Load Message Success': props<{ message: ChatMessage[] }>(),
    'Load Message Failure': props<{ error: string }>()
  },
});
