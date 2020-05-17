import { combineReducers } from "redux";
import open_thread from "./OpenThread";
import suggestions from "./Suggestions";

export const single_thread = combineReducers({
  open_thread: open_thread,
  suggestions: suggestions,
});
