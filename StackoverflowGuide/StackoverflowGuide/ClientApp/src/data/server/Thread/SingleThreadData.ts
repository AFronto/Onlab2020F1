import ThreadData from "./ThreadData";
import PostData from "../Post/PostData";

export default interface SingleThreadData {
  thread: ThreadData;
  posts: PostData[];
}
