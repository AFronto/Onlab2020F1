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
        !links.some((link) => link.source === cP && link.target === post.id) &&
        posts.some((targetPost) => cP === targetPost.id)
      ) {
        links.push({ source: post.id, target: cP });
      }
    });
  });
  return links;
}

export function graphData(postsData: PostData[]) {
  var x = 200;
  var y = 100;
  var x_wiggle = -40;
  var xOffset = [-30, -30, 30];
  postsData = postsData
    .slice(3)
    .map((pD) => {
      y += 15;
      x += x_wiggle;
      x_wiggle *= -1;
      return { ...pD, fx: x, fy: y };
    })
    .concat(
      postsData.slice(0, 3).map((pD) => {
        x += xOffset.pop()!;
        y += 15;
        return { ...pD, fx: x, fy: y };
      })
    );
  console.log(postsData);
  return {
    nodes: postsData,
    links: generateLinks(postsData),
    focusedNodeId: postsData.find(
      (p) => p.threadIndex === 0 || p.threadIndex === -1
    ),
  };
}
