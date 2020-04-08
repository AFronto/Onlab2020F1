import React, { FunctionComponent, useState } from "react";

import { Card, Button, Badge, Modal, Form } from "react-bootstrap";
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

  const [show, setShow] = useState(false);

  const handleClose = () => setShow(false);
  const handleShow = () => setShow(true);

  return (
    <>
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
              handleShow();
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

      <Modal show={show} onHide={handleClose}>
        <Modal.Header closeButton>
          <Modal.Title>Edit the Thread</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <>
            <Form>
              <Form.Group controlId="formThreadName">
                <Form.Label>Thread Name</Form.Label>
                <Form.Control type="email" placeholder={props.thread.name} />
              </Form.Group>

              <Form.Group controlId="formThreadTags">
                <Form.Label>Tags</Form.Label>
                <Form.Control type="email" placeholder="Password" />
              </Form.Group>
            </Form>
          </>
        </Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={handleClose}>
            Close
          </Button>
          <Button
            variant="primary"
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
              handleClose();
            }}
          >
            Save Changes
          </Button>
        </Modal.Footer>
      </Modal>
    </>
  );
};
