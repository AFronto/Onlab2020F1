import { createSlice } from "@reduxjs/toolkit";
import SinglePostData from "../../../data/server/Post/SinglePostData";

const openPostSlice = createSlice({
  name: "open_post",
  initialState: {} as SinglePostData,
  reducers: {
    loadPost(_state, action) {
      return action.payload.post;
    },
  },
});

export const { loadPost } = openPostSlice.actions;
export default openPostSlice.reducer;
