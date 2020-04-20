import { combineReducers, getDefaultMiddleware } from "@reduxjs/toolkit";
import { configureStore } from "@reduxjs/toolkit";
import thunk from "redux-thunk";
import history from "./applcationHistory";
import { connectRouter, routerMiddleware } from "connected-react-router";
import threads from "./Thread";
import open_thread from "./Thread/OpenThread";
import jwt from "./Auth";
import errors from "./Errors";
import tags from "./Tag";

const rootReducer = combineReducers({
  router: connectRouter(history),
  jwt: jwt,
  errors: errors,
  threads: threads,
  tags: tags,
  open_thread: open_thread,
});

const middleware = [
  ...getDefaultMiddleware(),
  thunk,
  routerMiddleware(history),
];

export const store = configureStore({
  reducer: rootReducer,
  middleware,
});

export type AppDispatch = typeof store.dispatch;

export type ReduxState = ReturnType<typeof store.getState>;
