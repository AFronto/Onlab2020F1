import { AppDispatch } from "../store";
import { removeAuthData } from "../store/Auth";
import { push } from "connected-react-router";

export const refreshInterval: { interval: NodeJS.Timeout; isSet: boolean } = {
  interval: setInterval(() => {}, 1000),
  isSet: false,
};

export function isLoggedIn(): boolean {
  const jwtToken = localStorage.getItem("jwtToken");
  return jwtToken !== undefined && jwtToken !== null;
}

export function logOut(dispatch: AppDispatch) {
  clearInterval(refreshInterval.interval);
  refreshInterval.isSet = false;
  dispatch(removeAuthData());
  dispatch(push("/login"));
}
