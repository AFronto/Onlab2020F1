import { createSlice } from "@reduxjs/toolkit";
import AuthData from "../../data/Auth/AuthData";

const authSlice = createSlice({
  name: "jwt",
  initialState: {} as AuthData,
  reducers: {
    loadAuthData(_state, action) {
      localStorage.setItem("jwtToken", action.payload.jwt.token);
      localStorage.setItem(
        "jwtTokenExpirationTime",
        action.payload.jwt.tokenExpirationTime
      );
      localStorage.setItem("jwtId", action.payload.jwt.id);
      return action.payload.jwt;
    }
  }
});

export const { loadAuthData } = authSlice.actions;
export default authSlice.reducer;
