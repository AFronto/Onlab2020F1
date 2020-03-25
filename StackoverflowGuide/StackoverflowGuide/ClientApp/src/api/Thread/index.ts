import axios from "axios";
import ThreadData from "../../data/Thread/ThreadData";
import { AppDispatch, ReduxState } from "../../store";
import { loadThreads } from "../../store/Thread";
import { generateAuthenticationHeadder } from "../Helpers/HeaderHelper";
import { replace } from "connected-react-router";
import { addError } from "../../store/Errors";
import { jwtExpires } from "../Helpers/JWTExpireHelper";

export function getThreads() {
  return (dispatch: AppDispatch, getState: () => ReduxState) => {
    const header = generateAuthenticationHeadder(getState());

    return axios({
      method: "GET",
      url: "thread",
      headers: header
    }).then(
      success => dispatch(loadThreads({ threadList: success.data })),
      error => {
        if (error.response.status === 401) {
          jwtExpires(dispatch);
        }
      }
    );
  };
}

export function createNewThread(threadData: ThreadData) {
  return (dispatch: AppDispatch, getState: () => ReduxState) => {
    const header = generateAuthenticationHeadder(getState());

    return axios({
      method: "POST",
      url: "thread/create",
      headers: header,
      data: threadData
    }).then(
      success => console.log(success), ///refactor, too server relient!!
      error => {
        if (error.response.status === 401) {
          jwtExpires(dispatch);
        }
      }
    );
  };
}

export function deleteThread(threadData: ThreadData) {}
