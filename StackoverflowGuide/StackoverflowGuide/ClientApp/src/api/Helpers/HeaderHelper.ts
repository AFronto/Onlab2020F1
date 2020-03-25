import { ReduxState } from "../../store";

export function generateAuthenticationHeadder(state: ReduxState) {
  const jwtToken = state.jwt.token;

  return {
    Authorization: "Bearer " + jwtToken
  };
}
