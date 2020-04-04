import { AppDispatch } from "../store";
import { removeAuthData } from "../store/Auth";
import { push } from "connected-react-router";

export function isLoggedIn(): boolean {
  const jwtToken = localStorage.getItem("jwtToken");
  return jwtToken !== undefined && jwtToken !== null;
}

export function logOut(dispatch: AppDispatch) {
  dispatch(removeAuthData());
  dispatch(push("/login"));
}
