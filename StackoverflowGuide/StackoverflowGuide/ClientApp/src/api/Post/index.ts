import { AppDispatch, ReduxState } from "../../store";
import { generateAuthenticationHeadder } from "../Helpers/HeaderHelper";
import axios from "axios";
import { jwtExpires } from "../Helpers/JWTExpireHelper";
import { loadSuggestions } from "../../store/Thread/SingleThread/Suggestions";

export function getSuggestions(id: String) {
  return (dispatch: AppDispatch, getState: () => ReduxState) => {
    const header = generateAuthenticationHeadder(getState());

    return axios({
      method: "GET",
      url: `post/suggestions/${id}`,
      headers: header,
    }).then(
      (success) => dispatch(loadSuggestions({ suggestions: success.data })),
      (error) => {
        if (error.response.status === 401) {
          jwtExpires(dispatch);
        }
      }
    );
  };
}
