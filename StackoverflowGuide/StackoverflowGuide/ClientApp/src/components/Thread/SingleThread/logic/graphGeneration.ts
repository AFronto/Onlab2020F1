import PostData from "../../../../data/server/Post/PostData";
import SingleThreadData from "../../../../data/server/Thread/SingleThreadData";

export function customLabelBuilder(node: PostData) {
  return node.title;
}

function generateLinks(
  posts: PostData[]
): { source: string; target: string }[] {
  const links: { source: string; target: string }[] = [];
  posts.forEach((post) => {
    post.connectedPosts.forEach((cP) => {
      if (
        !links.some((link) => link.source === cP && link.target === post.id)
      ) {
        links.push({ source: post.id, target: cP });
      }
    });
  });
  console.log(links);
  return links;
}

export function graphData(open_thread: SingleThreadData) {
  return {
    nodes: open_thread.posts,
    links: generateLinks(open_thread.posts),
    focusedNodeId: open_thread.posts.find((p) => p.threadIndex === 0),
  };
}
