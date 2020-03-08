import { createSlice } from "@reduxjs/toolkit";
import Thread from "../../data/Threads/Thread";

const threadsSlice = createSlice({
  name: "threads",
  initialState: [] as Thread[],
  reducers: {
    loadThreads(_state, action) {
      return action.payload.threadList;
    },
    addThread(state, action) {
      state.push(action.payload.newThread);
      return state;
    }
  }
});

export const { loadThreads, addThread } = threadsSlice.actions;
export default threadsSlice.reducer;
