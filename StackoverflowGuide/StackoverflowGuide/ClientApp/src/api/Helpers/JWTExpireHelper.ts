import { AppDispatch } from "../../store";
import { addError } from "../../store/Errors";
import { logOut } from "../../general_helpers/AuthHelper";

export function jwtExpires(dispatch: AppDispatch) {
  dispatch(
    addError({
      name: "credentialError",
      description: "Your JWT token has expired"
    })
  );
  logOut(dispatch);
}
