import PostData from "../../../../data/server/Post/PostData";

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
  return links;
}

export function graphData(postsData: PostData[]) {
  return {
    nodes: postsData,
    links: generateLinks(postsData),
    focusedNodeId: postsData.find(
      (p) => p.threadIndex === 0 || p.threadIndex === -1
    ),
  };
}
