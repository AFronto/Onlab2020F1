import axios from "axios";
import ThreadData from "../../data/Thread/ThreadData";
import { AppDispatch } from "../../store";

export function getThreads() {}

export function createNewThread(threadData: ThreadData) {
  return (dispatch: AppDispatch) => {
    return axios.post("thread/create", threadData).then(
      success => console.log(success),
      error => console.log(error)
    );
  };
}

export function deleteThread(threadData: ThreadData) {}
