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
import { loadSingleThread } from "../../store/Thread/SingleThread/OpenThread";
import { loadAllTags } from "../../store/Tag";
import { loadSuggestions } from "../../store/Thread/SingleThread/Suggestions";
import SingleThreadData from "../../data/server/Thread/SingleThreadData";
import SearchData from "../../data/server/Thread/SearchData";

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
        console.log(error.response);
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

export function getSingleThread(id: string) {
  return (dispatch: AppDispatch, getState: () => ReduxState) => {
    const header = generateAuthenticationHeadder(getState());

    return axios({
      method: "GET",
      url: "thread/" + id,
      headers: header,
    }).then(
      (success) => {
        dispatch(
          loadSingleThread({
            singleThread: {
              thread: success.data.thread,
              posts: success.data.posts,
            } as SingleThreadData,
          })
        );
        dispatch(loadSuggestions({ suggestions: success.data.suggestions }));
      },
      (error) => {
        if (error.response.status === 401) {
          jwtExpires(dispatch);
        }
      }
    );
  };
}

export function getAllTags() {
  return (dispatch: AppDispatch, getState: () => ReduxState) => {
    const header = generateAuthenticationHeadder(getState());

    return axios({
      method: "GET",
      url: "thread/tags",
      headers: header,
    }).then(
      (success) => dispatch(loadAllTags({ tags: success.data })),
      (error) => {
        if (error.response.status === 401) {
          jwtExpires(dispatch);
        }
      }
    );
  };
}

export function sendSearch(id: string, searchTerm: SearchData) {
  return (dispatch: AppDispatch, getState: () => ReduxState) => {
    const header = generateAuthenticationHeadder(getState());
    return axios({
      method: "POST",
      url: "thread/" + id + "/search",
      headers: header,
      data: searchTerm,
    }).then(
      (success) => {
        dispatch(
          loadSingleThread({
            singleThread: {
              thread: success.data.thread,
              posts: success.data.posts,
            } as SingleThreadData,
          })
        );
        dispatch(loadSuggestions({ suggestions: success.data.suggestions }));
      },
      (error) => {
        if (error.response.status === 401) {
          jwtExpires(dispatch);
        }
      }
    );
  };
}
