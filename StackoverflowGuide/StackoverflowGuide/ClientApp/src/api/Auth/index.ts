import axios from "axios";
import LoginData from "../../data/Auth/LoginData";
import RegisterData from "../../data/Auth/RegisterData";

export function login(loginData: LoginData) {
  axios.post("auth/login", loginData);
}

export function register(registerData: RegisterData) {
  axios.post("auth/login", registerData);
}
