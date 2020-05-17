import { createSlice } from "@reduxjs/toolkit";
import SingleThreadData from "../../../data/server/Thread/SingleThreadData";

const openThreadSlice = createSlice({
  name: "open_thread",
  initialState: {} as SingleThreadData,
  reducers: {
    loadSingleThread(_state, action) {
      return action.payload.singleThread;
    },
  },
});

export const { loadSingleThread } = openThreadSlice.actions;
export default openThreadSlice.reducer;
