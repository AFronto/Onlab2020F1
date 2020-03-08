import { combineReducers, getDefaultMiddleware } from "@reduxjs/toolkit";
import { configureStore } from "@reduxjs/toolkit";
import thunk from "redux-thunk";
import history from "./applcationHistory";
import { connectRouter, routerMiddleware } from "connected-react-router";
import counter from "./Counter";
import threads from "./Threads/index";

const rootReducer = combineReducers({
  router: connectRouter(history),
  counter: counter,
  threads: threads
});

const middleware = [
  ...getDefaultMiddleware(),
  thunk,
  routerMiddleware(history)
];

export const store = configureStore({
  reducer: rootReducer,
  middleware
});

export type ReduxState = ReturnType<typeof store.getState>;
