import { createSlice } from "@reduxjs/toolkit";
import PostData from "../../../data/server/Post/PostData";

const suggestionsSlice = createSlice({
  name: "suggestions",
  initialState: [] as PostData[],
  reducers: {
    loadSuggestions(_state, action) {
      return action.payload.suggestions;
    },
  },
});

export const { loadSuggestions } = suggestionsSlice.actions;
export default suggestionsSlice.reducer;
