import React, { FunctionComponent } from "react";

import { Card, Button, Badge } from "react-bootstrap";
import ThreadData from "../../data/Thread/ThreadData";
import { useDispatch } from "react-redux";
import { removeThread } from "../../store/Thread";
import { updateThread } from "../../store/Thread";
import { deleteThread } from "../../api/Thread";
import { editThread } from "../../api/Thread";

export const ThreadCard: FunctionComponent<{ thread: ThreadData }> = (
  props
) => {
  const dispatch = useDispatch();

  return (
    <Card style={{ width: "18rem", marginBottom: 40 }}>
      <Card.Body>
        <Card.Title>{props.thread.name}</Card.Title>

        <h4>
          {props.thread.tagList.map((tag) => (
            <Badge style={{ margin: 5 }} pill color="secondary">
              {tag}
            </Badge>
          ))}
        </h4>
        <Button
          onClick={() => {
            dispatch(
              editThread(props.thread, {
                id: props.thread.id,
                name: "Edit",
                tagList: ["Edit", "C++"],
              })
            );
            dispatch(
              updateThread({
                id: props.thread.id,
                name: "EDit",
                tagList: ["Edit", "C++"],
              })
            );
          }}
        >
          Edit
        </Button>
        <Button
          onClick={() => {
            dispatch(removeThread({ threadId: props.thread.id }));
            dispatch(deleteThread(props.thread));
          }}
        >
          Delete
        </Button>
      </Card.Body>
    </Card>
  );
};
