import React, { FunctionComponent, useState, useCallback } from "react";
import { FaTrash, FaPencilAlt } from "react-icons/fa";

import { Card, Button, Badge } from "react-bootstrap";
import ThreadData from "../../data/server/Thread/ThreadData";
import { useDispatch } from "react-redux";
import { removeThread } from "../../store/Thread";
import { deleteThread } from "../../api/Thread";
import { ThreadModal } from "./ThreadModal";
import { push } from "connected-react-router";

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
              <Badge style={{ margin: 5 }} pill variant="warning">
                {tag}
              </Badge>
            ))}
          </h4>
          <div className="d-flex justify-content-between">
            <Button
              variant="outline-success"
              className="border border-success"
              onClick={() => dispatch(push(`/threads/${props.thread.id}`))}
            >
              Show
            </Button>
            <div className="d-flex justify-content-end">
              <Button
                variant="outline-primary"
                className="border border-primary mr-2"
                onClick={() => {
                  handleShow();
                }}
              >
                <FaPencilAlt />
              </Button>
              <Button
                variant="outline-danger"
                className="border border-danger"
                onClick={() => {
                  dispatch(removeThread({ threadId: props.thread.id }));
                  dispatch(deleteThread(props.thread));
                }}
              >
                <FaTrash />
              </Button>
            </div>
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
