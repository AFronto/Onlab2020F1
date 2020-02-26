import { createSlice } from "@reduxjs/toolkit";

const threadsSlice = createSlice({
  name: "threads",
  initialState: { thread: [{ id: 0, name: "test", tagList: ["test"] }] },
  reducers: {}
});

export const {} = threadsSlice.actions;
export default threadsSlice.reducer;
