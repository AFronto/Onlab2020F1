import { AppDispatch, ReduxState } from "../../store";
import { generateAuthenticationHeadder } from "../Helpers/HeaderHelper";
import axios from "axios";
import { jwtExpires } from "../Helpers/JWTExpireHelper";
import { loadSuggestions } from "../../store/Thread/SingleThread/Suggestions";
import PostData from "../../data/server/Post/PostData";
import { loadSingleThread } from "../../store/Thread/SingleThread/OpenThread";
import SingleThreadData from "../../data/server/Thread/SingleThreadData";
import { loadPost } from "../../store/Thread/OpenPost";

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

export function getSinglePost(threadId: string, postId: string) {
  return (dispatch: AppDispatch, getState: () => ReduxState) => {
    const header = generateAuthenticationHeadder(getState());

    return axios({
      method: "GET",
      url: `post/${threadId}/get/${postId}`,
      headers: header,
    }).then(
      (success) => {
        console.log(success.data);
        dispatch(loadPost({ post: success.data }));
      },
      (error) => {
        if (error.response.status === 401) {
          jwtExpires(dispatch);
        }
      }
    );
  };
}
