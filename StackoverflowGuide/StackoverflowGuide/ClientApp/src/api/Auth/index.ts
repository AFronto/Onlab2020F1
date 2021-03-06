import axios from "axios";
import LoginData from "../../data/server/Auth/LoginData";
import RegisterData from "../../data/server/Auth/RegisterData";
import { AppDispatch, ReduxState } from "../../store";
import { loadAuthData } from "../../store/Auth";
import { addError } from "../../store/Errors";
import { push } from "connected-react-router";
import { generateAuthenticationHeadder } from "../Helpers/HeaderHelper";
import { logOut, refreshInterval } from "../../general_helpers/AuthHelper";

export function login(loginData: LoginData) {
  return (dispatch: AppDispatch) => {
    return axios.post("auth/login", loginData).then(
      (success) => {
        dispatch(loadAuthData({ jwt: success.data }));
        dispatch(push("/threads"));
      },
      (error) =>
        dispatch(
          addError({
            name: "credentialError",
            description: error.response.data.error,
          })
        )
    );
  };
}

export function createNewAccount(registerData: RegisterData) {
  return (dispatch: AppDispatch) => {
    return axios.post("auth/register", registerData).then(
      (success) => {
        dispatch(loadAuthData({ jwt: success.data }));
        dispatch(push("/threads"));
      },
      (error) =>
        dispatch(
          addError({
            name: "registrationCredentialError",
            description: error.response.data.error,
          })
        )
    );
  };
}

export function initializeScreen() {
  return (dispatch: AppDispatch, getState: () => ReduxState) => {
    if (refreshInterval.isSet === false) {
      refreshInterval.isSet = true;
      refreshToken(dispatch, getState);
      refreshInterval.interval = setInterval(() => {
        refreshToken(dispatch, getState);
      }, 5000);
    }
  };
}

function refreshToken(dispatch: AppDispatch, getState: () => ReduxState) {
  const header = generateAuthenticationHeadder(getState());
  return axios({
    method: "POST",
    url: "auth/token",
    headers: header,
  }).then(
    (success) => {
      dispatch(loadAuthData({ jwt: success.data }));
    },
    (error) => {
      dispatch(
        addError({
          name: "refreshError",
          description: error.response.data.error,
        })
      );
      logOut(dispatch);
    }
  );
}
