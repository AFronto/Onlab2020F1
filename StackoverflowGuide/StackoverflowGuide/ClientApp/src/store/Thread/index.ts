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
    removeThread(state, action) {
      console.log(state);
      console.log("test");
      console.log(action.payload.threadId);
      var threadToDelete = state.find(
        thread => thread.id === action.payload.threadId
      );
      if (threadToDelete !== undefined) {
        state.splice(state.indexOf(threadToDelete), 1);
      }
      return state;
    }
  }
});

export const { loadThreads, addThread, removeThread } = threadsSlice.actions;
export default threadsSlice.reducer;
