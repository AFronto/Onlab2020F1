import { createSlice } from "@reduxjs/toolkit";
import AuthData from "../../data/Auth/AuthData";

const authSlice = createSlice({
  name: "jwt",
  initialState: {} as AuthData,
  reducers: {
    loadAuthData(_state, action) {
      return action.payload.jwt;
    }
  }
});

export const { loadAuthData } = authSlice.actions;
export default authSlice.reducer;
