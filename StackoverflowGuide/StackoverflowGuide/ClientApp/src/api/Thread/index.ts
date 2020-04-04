import axios from "axios";
import ThreadData from "../../data/Thread/ThreadData";
import { AppDispatch, ReduxState } from "../../store";
import {
  loadThreads,
  updateThread,
  removeThread,
  addThread,
} from "../../store/Thread";
import { generateAuthenticationHeadder } from "../Helpers/HeaderHelper";
import { jwtExpires } from "../Helpers/JWTExpireHelper";

export function getThreads() {
  return (dispatch: AppDispatch, getState: () => ReduxState) => {
    const header = generateAuthenticationHeadder(getState());

    return axios({
      method: "GET",
      url: "thread",
      headers: header,
    }).then(
      (success) => dispatch(loadThreads({ threadList: success.data })),
      (error) => {
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
      data: threadData,
    }).then(
      (success) => {
        dispatch(
          updateThread({
            threadId: threadData.id,
            updatedThread: { ...threadData, id: success.data.id },
          })
        );
      },
      (error) => {
        dispatch(removeThread({ threadId: threadData.id }));
        if (error.response.status === 401) {
          jwtExpires(dispatch);
        }
      }
    );
  };
}

export function deleteThread(threadData: ThreadData) {
  return (dispatch: AppDispatch, getState: () => ReduxState) => {
    const header = generateAuthenticationHeadder(getState());

    return axios({
      method: "DELETE",
      url: "thread/" + threadData.id,
      headers: header,
    }).then(
      (success) => {
        console.log(success.data.threadId);
      },
      (error) => {
        dispatch(addThread({ newThread: threadData }));
        if (error.response.status === 401) {
          jwtExpires(dispatch);
        }
      }
    );
  };
}
