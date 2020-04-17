export default interface PostData {
  id: string;
  threadIndex: number;
  title: string;
  body: string;
  connectedPosts: string[];
}
