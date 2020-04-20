import { createSlice } from "@reduxjs/toolkit";
import TagData from "../../data/server/Tag/TagData";

const allTagsSlice = createSlice({
  name: "tags",
  initialState: [] as TagData[],
  reducers: {
    loadAllTags(_state, action) {
      return action.payload.tags;
    },
  },
});

export const { loadAllTags } = allTagsSlice.actions;
export default allTagsSlice.reducer;
