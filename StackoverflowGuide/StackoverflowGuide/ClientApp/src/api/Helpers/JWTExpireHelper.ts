import { AppDispatch } from "../../store";
import { addError } from "../../store/Errors";
import { replace } from "connected-react-router";

export function jwtExpires(dispatch: AppDispatch) {
  dispatch(
    addError({
      name: "credentialError",
      description: "Your JWT token has expired"
    })
  );
  dispatch(replace("/login"));
}
