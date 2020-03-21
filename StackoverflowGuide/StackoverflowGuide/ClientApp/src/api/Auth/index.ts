import axios from "axios";
import LoginData from "../../data/Auth/LoginData";
import RegisterData from "../../data/Auth/RegisterData";
import { AppDispatch } from "../../store";
import { loadAuthData } from "../../store/Auth";
import { addError } from "../../store/Errors";

export function login(loginData: LoginData) {
  return (dispatch: AppDispatch) => {
    return axios.post("auth/login", loginData).then(
      success => dispatch(loadAuthData({ jwt: success.data })),
      error =>
        dispatch(
          addError({
            name: "credentialError",
            description: error.response.data.error
          })
        )
    );
  };
}

export function createNewAccount(registerData: RegisterData) {
  return (dispatch: AppDispatch) => {
    return axios.post("auth/register", registerData).then(
      success => dispatch(loadAuthData({ jwt: success.data })),
      error => console.log(error.response.data.error)
    );
  };
}
