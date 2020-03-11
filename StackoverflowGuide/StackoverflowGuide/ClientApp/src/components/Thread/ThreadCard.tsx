import React, { FunctionComponent } from "react";

import { Card, Button, Badge } from "react-bootstrap";
import ThreadData from "../../data/Thread/ThreadData";
import { useDispatch } from "react-redux";
import { removeThread } from "../../store/Thread";

export const ThreadCard: FunctionComponent<{ thread: ThreadData }> = props => {
  const dispatch = useDispatch();

  return (
    <Card>
      <Card.Body>
        <Card.Title>{props.thread.name}</Card.Title>

        <h4>
          {props.thread.tagList.map(tag => (
            <Badge style={{ margin: 5 }} pill color="secondary">
              {tag}
            </Badge>
          ))}
        </h4>
        <Button>Edit</Button>
        <Button
          onClick={() => {
            dispatch(removeThread({ threadId: props.thread.id }));
          }}
        >
          Delete
        </Button>
      </Card.Body>
    </Card>
  );
};
