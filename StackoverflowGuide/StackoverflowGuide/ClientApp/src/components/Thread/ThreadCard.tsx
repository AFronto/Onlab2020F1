import React, { FunctionComponent, useState, useCallback } from "react";
import { FaTrash, FaPencilAlt } from "react-icons/fa";

import { Card, Button, Badge } from "react-bootstrap";
import ThreadData from "../../data/server/Thread/ThreadData";
import { useDispatch } from "react-redux";
import { removeThread } from "../../store/Thread";
import { deleteThread } from "../../api/Thread";
import { ThreadModal } from "./ThreadModal";

export const ThreadCard: FunctionComponent<{ thread: ThreadData }> = (
  props
) => {
  const dispatch = useDispatch();

  const [show, setShow] = useState(false);

  const handleClose = useCallback(() => setShow(false), [setShow]);
  const handleShow = () => setShow(true);

  return (
    <>
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
                handleShow();
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

      <ThreadModal
        model={{ show, handleClose }}
        isNew={false}
        thread={props.thread}
      />
    </>
  );
};
