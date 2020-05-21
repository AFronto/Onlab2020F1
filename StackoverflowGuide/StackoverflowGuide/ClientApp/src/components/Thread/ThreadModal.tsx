import React, { FunctionComponent, useState, useEffect } from "react";
import ThreadData from "../../data/server/Thread/ThreadData";
import { Modal, Form, Button, Spinner } from "react-bootstrap";
import { useDispatch, useSelector } from "react-redux";
import { updateThread, addThread } from "../../store/Thread";
import { editThread, createNewThread, getAllTags } from "../../api/Thread";
import ModalModel from "../../data/client/Modal/ModalModel";
import { useForm } from "react-hook-form";
import * as yup from "yup";
import { isArray } from "util";
import { Typeahead } from "react-bootstrap-typeahead";
import { ReduxState } from "../../store";
import TagData from "../../data/server/Tag/TagData";
import { loadAllTags } from "../../store/Tag";

export const ThreadModal: FunctionComponent<{
  model: ModalModel;
  isNew: boolean;
  thread?: ThreadData;
}> = (props) => {
  const { show, handleClose } = props.model;
  const [selected, setSelected] = useState([] as TagData[]);
  const dispatch = useDispatch();

  const tags = useSelector((state: ReduxState) => state.tags);

  useEffect(() => {
    if (tags.length === 0 && show) {
      dispatch(getAllTags());
    } else {
      dispatch(loadAllTags({ tags: [] }));
    }

    return function cleanup() {
      dispatch(loadAllTags({ tags: [] }));
    };
  }, [show]);

  const schema = yup.object({
    name: yup.string().required(),
  });

  const { register, handleSubmit, errors } = useForm({
    validationSchema: schema,
  });

  const onSubmit = handleSubmit((data) => {
    const threadToSend = {
      id: props.thread ? props.thread.id : "fake_id",
      name: data.name,
      tagList: selected.map((tag) => tag.name),
    };

    if (props.isNew) {
      dispatch(
        addThread({
          newThread: threadToSend,
        })
      );
      dispatch(createNewThread(threadToSend));
    } else {
      dispatch(editThread(props.thread!, threadToSend));
      dispatch(
        updateThread({
          threadId: props.thread!.id,
          updatedThread: threadToSend,
        })
      );
    }
    handleClose();
  });

  var modalTitle = props.isNew ? "Create a new Thread" : "Edit the Thread";

  var submitButton = props.isNew ? (
    <Button
      variant="outline-success"
      className="border border-success"
      type="submit"
    >
      Create Thread
    </Button>
  ) : (
    <Button
      variant="outline-primary"
      className="border border-primary"
      type="submit"
    >
      Save Changes
    </Button>
  );

  return (
    <Modal centered show={show} onHide={handleClose}>
      <Modal.Header closeButton>
        <Modal.Title>{modalTitle}</Modal.Title>
      </Modal.Header>
      {tags.length > 0 ? (
        <Modal.Body>
          <Form noValidate onSubmit={onSubmit}>
            <Form.Group controlId="formThreadName">
              <Form.Label>Thread Name</Form.Label>
              <Form.Control
                name="name"
                type="text"
                defaultValue={props.thread ? props.thread.name : ""}
                ref={register}
                isInvalid={!!errors.name}
                placeholder="Name of your Thread..."
              />
              <Form.Control.Feedback type="invalid">
                <h6>
                  {errors.name
                    ? isArray(errors.name)
                      ? errors.name[0].message
                      : errors.name.message
                    : ""}
                </h6>
              </Form.Control.Feedback>
            </Form.Group>

            <Form.Group controlId="formThreadTags">
              <Form.Label>Tags</Form.Label>
              <div className="clearfix">
                <Typeahead
                  id="tag-selection"
                  labelKey="name"
                  multiple={true}
                  defaultSelected={tags.filter(
                    (tag) =>
                      props.thread && props.thread.tagList.includes(tag.name)
                  )}
                  options={tags}
                  onChange={setSelected}
                  placeholder="Choose a tag..."
                />
              </div>
            </Form.Group>

            <div className="d-flex justify-content-end">
              {submitButton}
              <Button
                variant="outline-danger"
                className="border border-danger ml-2"
                onClick={handleClose}
              >
                Close
              </Button>
            </div>
          </Form>
        </Modal.Body>
      ) : (
        <div className="d-flex justify-content-center">
          <Spinner animation="border" />
        </div>
      )}
      <Modal.Footer></Modal.Footer>
    </Modal>
  );
};
