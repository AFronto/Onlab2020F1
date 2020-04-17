import PostData from "../../../data/server/Post/PostData";
import React, { FunctionComponent } from "react";
import { Card } from "react-bootstrap";

export const PostdCard: FunctionComponent<{ post: PostData }> = (props) => {
  return (
    <Card
      style={{ width: "18rem", marginBottom: 40 }}
      className="mx-auto"
      border="primary"
    >
      <Card.Body>
        <Card.Title>{props.post.title}</Card.Title>
      </Card.Body>
    </Card>
  );
};
