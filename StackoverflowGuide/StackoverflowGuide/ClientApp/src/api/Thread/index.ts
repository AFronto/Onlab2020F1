import axios from "axios";
import ThreadData from "../../data/Thread/ThreadData";
import { AppDispatch } from "../../store";
import { loadThreads } from "../../store/Thread";

export function getThreads() {
  return (dispatch: AppDispatch) => {
    return axios.get("thread").then(
      success => dispatch(loadThreads({ threadList: success.data })),
      error => console.log(error)
    );
  };
}

export function createNewThread(threadData: ThreadData) {
  return (dispatch: AppDispatch) => {
    return axios.post("thread/create", threadData).then(
      success => dispatch(getThreads()), ///refactor, too server relient!!
      error => console.log(error)
    );
  };
}

export function deleteThread(threadData: ThreadData) {}
