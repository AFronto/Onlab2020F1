import { createSlice } from "@reduxjs/toolkit";
import AuthData from "../../data/server/Auth/AuthData";

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
    },
    removeAuthData() {
      localStorage.removeItem("jwtToken");
      localStorage.removeItem("jwtId");
      localStorage.removeItem("jwtTokenExpirationTime");
      return {} as AuthData;
    },
  },
});

export const { removeAuthData, loadAuthData } = authSlice.actions;
export default authSlice.reducer;
