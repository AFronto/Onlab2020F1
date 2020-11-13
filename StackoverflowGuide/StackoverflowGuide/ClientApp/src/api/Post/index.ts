import { AppDispatch, ReduxState } from "../../store";
import { generateAuthenticationHeadder } from "../Helpers/HeaderHelper";
import axios from "axios";
import { jwtExpires } from "../Helpers/JWTExpireHelper";
import { loadSuggestions } from "../../store/Thread/SingleThread/Suggestions";
import PostData from "../../data/server/Post/PostData";
import { loadSingleThread } from "../../store/Thread/SingleThread/OpenThread";
import SingleThreadData from "../../data/server/Thread/SingleThreadData";

export function getSuggestionsAfterDecline(
  id: string,
  oldState: PostData[],
  declinedPost: PostData
) {
  return (dispatch: AppDispatch, getState: () => ReduxState) => {
    const header = generateAuthenticationHeadder(getState());

    return axios({
      method: "POST",
      url: `post/suggestions/${id}/declined`,
      headers: header,
      data: declinedPost,
    }).then(
      (success) => dispatch(loadSuggestions({ suggestions: success.data })),
      (error) => {
        dispatch(
          loadSuggestions({
            suggestions: oldState,
          })
        );
        if (error.response.status === 401) {
          jwtExpires(dispatch);
        }
      }
    );
  };
}

export function getSuggestionsAfterAccept(
  id: string,
  oldState: { suggestions: PostData[]; singleThread: SingleThreadData },
  acceptedPost: PostData
) {
  return (dispatch: AppDispatch, getState: () => ReduxState) => {
    const header = generateAuthenticationHeadder(getState());

    return axios({
      method: "POST",
      url: `post/suggestions/${id}/accepted`,
      headers: header,
      data: acceptedPost,
    }).then(
      (success) => {
        dispatch(loadSuggestions({ suggestions: success.data.suggestions }));
        dispatch(
          loadSingleThread({
            singleThread: {
              thread: oldState.singleThread.thread,
              posts: oldState.singleThread.posts.concat([success.data.newPost]),
            } as SingleThreadData,
          })
        );
      },
      (error) => {
        dispatch(
          loadSuggestions({
            suggestions: oldState.suggestions,
          })
        );
        dispatch(
          loadSingleThread({
            singleThread: oldState.singleThread,
          })
        );
        if (error.response.status === 401) {
          jwtExpires(dispatch);
        }
      }
    );
  };
}

export function deleteWatched(
  id: string,
  oldState: { suggestions: PostData[]; singleThread: SingleThreadData },
  deletedPost: PostData
) {
  return (dispatch: AppDispatch, getState: () => ReduxState) => {
    const header = generateAuthenticationHeadder(getState());

    return axios({
      method: "DELETE",
      url: `post/${id}/delete/${deletedPost.id}`,
      headers: header,
    }).then(
      (_success) => {
        console.log("Succesfully deleted!");
      },
      (error) => {
        dispatch(
          loadSuggestions({
            suggestions: oldState.suggestions,
          })
        );
        dispatch(
          loadSingleThread({
            singleThread: oldState.singleThread,
          })
        );
        if (error.response.status === 401) {
          jwtExpires(dispatch);
        }
      }
    );
  };
}
