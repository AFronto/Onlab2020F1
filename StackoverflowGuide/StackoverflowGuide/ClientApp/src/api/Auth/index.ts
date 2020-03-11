import axios from "axios";
import LoginData from "../../data/Auth/LoginData";
import RegisterData from "../../data/Auth/RegisterData";
import { AppDispatch } from "../../store";

export function login(loginData: LoginData) {
  return (dispatch: AppDispatch) => {
    return axios.post("auth/login", loginData).then(
      success => console.log(success),
      error => console.log(error)
    );
  };
}

export function createNewAccount(registerData: RegisterData) {
  return (dispatch: AppDispatch) => {
    return axios.post("auth/register", registerData).then(
      success => console.log(success),
      error => console.log(error)
    );
  };
}
