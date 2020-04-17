import axios from "axios";
import ThreadData from "../../data/server/Thread/ThreadData";
import { AppDispatch, ReduxState } from "../../store";
import {
  loadThreads,
  updateThread,
  removeThread,
  addThread,
} from "../../store/Thread";
import { generateAuthenticationHeadder } from "../Helpers/HeaderHelper";
import { jwtExpires } from "../Helpers/JWTExpireHelper";
import { loadSingleThread } from "../../store/Thread/OpenThread";

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

export function editThread(
  originalthreadData: ThreadData,
  newThreadData: ThreadData
) {
  return (dispatch: AppDispatch, getState: () => ReduxState) => {
    const header = generateAuthenticationHeadder(getState());

    return axios({
      method: "PUT",
      url: "thread/" + newThreadData.id,
      headers: header,
      data: newThreadData,
    }).then(
      (success) => {
        console.log(success);
      },
      (error) => {
        dispatch(
          updateThread({
            threadId: originalthreadData.id,
            updatedThread: { ...originalthreadData, id: originalthreadData.id },
          })
        );
        if (error.response.status === 401) {
          jwtExpires(dispatch);
        }
      }
    );
  };
}

export function getSingleThread(id: String) {
  return (dispatch: AppDispatch, getState: () => ReduxState) => {
    const header = generateAuthenticationHeadder(getState());

    return axios({
      method: "GET",
      url: "thread/" + id,
      headers: header,
    }).then(
      (success) => dispatch(loadSingleThread({ singleThread: success.data })),
      (error) => {
        if (error.response.status === 401) {
          jwtExpires(dispatch);
        }
      }
    );
  };
}
