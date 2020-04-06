import { createSlice } from "@reduxjs/toolkit";
import ThreadData from "../../data/Thread/ThreadData";

const threadsSlice = createSlice({
  name: "threads",
  initialState: [] as ThreadData[],
  reducers: {
    loadThreads(_state, action) {
      return action.payload.threadList;
    },
    addThread(state, action) {
      state.push(action.payload.newThread);
      return state;
    },
    updateThread(state, action) {
      var threadToUpdateIndex = state.findIndex(
        (thread) => thread.id === action.payload.threadId
      );
      if (threadToUpdateIndex !== -1) {
        state[threadToUpdateIndex] = action.payload.updatedThread;
      }
      return state;
    },
    removeThread(state, action) {
      var threadToDeleteIndex = state.findIndex(
        (thread) => thread.id === action.payload.threadId
      );
      if (threadToDeleteIndex !== -1) {
        state.splice(threadToDeleteIndex, 1);
      }
      return state;
    },
  },
});

export const {
  loadThreads,
  addThread,
  updateThread,
  removeThread,
} = threadsSlice.actions;
export default threadsSlice.reducer;
