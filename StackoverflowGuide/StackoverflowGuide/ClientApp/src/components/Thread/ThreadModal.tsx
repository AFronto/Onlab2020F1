import React, { FunctionComponent } from "react";
import ThreadData from "../../data/server/Thread/ThreadData";
import { Modal, Form, Button } from "react-bootstrap";
import { useDispatch } from "react-redux";
import { updateThread } from "../../store/Thread";
import { editThread } from "../../api/Thread";
import ModalModel from "../../data/client/Modal/ModalModel";

export const ThreadModal: FunctionComponent<{
  model: ModalModel;
  isNew: boolean;
  thread: ThreadData;
}> = (props) => {
  const { show, handleClose } = props.model;
  const dispatch = useDispatch();

  return (
    <Modal show={show} onHide={handleClose}>
      <Modal.Header closeButton>
        <Modal.Title>Edit the Thread</Modal.Title>
      </Modal.Header>
      <Modal.Body>
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
  );
};
