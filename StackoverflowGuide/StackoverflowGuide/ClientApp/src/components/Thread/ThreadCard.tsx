import React, { FunctionComponent } from "react";
import { FaTrash, FaPencilAlt } from "react-icons/fa";

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
    <Card
      style={{ width: "18rem", marginBottom: 40 }}
      className="mx-auto"
      border="primary"
    >
      <Card.Header as="h5">{props.thread.name}</Card.Header>
      <Card.Body>
        <h4>
          {props.thread.tagList.map((tag) => (
            <Badge style={{ margin: 5 }} pill variant="secondary">
              {tag}
            </Badge>
          ))}
        </h4>
        <div className="d-flex justify-content-end">
          <Button
            className="mr-2"
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
                  threadId: props.thread.id,
                  updatedThread: {
                    id: props.thread.id,
                    name: "Edit",
                    tagList: ["Edit", "C++"],
                  },
                })
              );
            }}
          >
            <FaPencilAlt />
          </Button>
          <Button
            variant="danger"
            onClick={() => {
              dispatch(removeThread({ threadId: props.thread.id }));
              dispatch(deleteThread(props.thread));
            }}
          >
            <FaTrash />
          </Button>
        </div>
      </Card.Body>
    </Card>
  );
};
