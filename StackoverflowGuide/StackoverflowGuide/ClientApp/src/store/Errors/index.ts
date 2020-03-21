import { createSlice } from "@reduxjs/toolkit";

const authSlice = createSlice({
  name: "errors",
  initialState: {} as { [key: string]: string },
  reducers: {
    addError(state, action) {
      state[action.payload.name] = action.payload.description;
      return state;
    },
    clearError(state, action) {
      delete state[action.payload.name];
      return state;
    }
  }
});

export const { addError, clearError } = authSlice.actions;
export default authSlice.reducer;
