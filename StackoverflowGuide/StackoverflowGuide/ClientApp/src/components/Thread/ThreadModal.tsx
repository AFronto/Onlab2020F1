import React, { FunctionComponent, useState, Fragment } from "react";
import ThreadData from "../../data/server/Thread/ThreadData";
import { Modal, Form, Button } from "react-bootstrap";
import { useDispatch, useSelector } from "react-redux";
import { updateThread } from "../../store/Thread";
import { editThread } from "../../api/Thread";
import ModalModel from "../../data/client/Modal/ModalModel";
import { useForm } from "react-hook-form";
import * as yup from "yup";
import { isArray } from "util";
import { Typeahead } from "react-bootstrap-typeahead";
import { ReduxState } from "../../store";
import TagData from "../../data/server/Tag/TagData";

export const ThreadModal: FunctionComponent<{
  model: ModalModel;
  isNew: boolean;
  thread: ThreadData;
}> = (props) => {
  const { show, handleClose } = props.model;
  const [selected, setSelected] = useState([] as TagData[]);
  const dispatch = useDispatch();

  const tags = useSelector((state: ReduxState) => state.tags);

  const schema = yup.object({
    name: yup.string().required(),
  });

  const { register, handleSubmit, errors } = useForm({
    validationSchema: schema,
  });

  const onSubmit = handleSubmit((data) => {
    console.log();
    dispatch(
      editThread(props.thread, {
        id: props.thread.id,
        name: data.name,
        tagList: ["Edit", "C++"],
      })
    );
    dispatch(
      updateThread({
        threadId: props.thread.id,
        updatedThread: {
          id: props.thread.id,
          name: data.name,
          tagList: ["Edit", "C++"],
        },
      })
    );
    handleClose();
  });

  return (
    <Modal show={show} onHide={handleClose}>
      <Modal.Header closeButton>
        <Modal.Title>Edit the Thread</Modal.Title>
      </Modal.Header>
      <Modal.Body>
        <Form noValidate onSubmit={onSubmit}>
          <Form.Group controlId="formThreadName">
            <Form.Label>Thread Name</Form.Label>
            <Form.Control
              name="name"
              type="text"
              defaultValue={props.thread.name}
              ref={register}
              isInvalid={!!errors.name}
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
            <Fragment>
              <div className="clearfix">
                <Typeahead
                  id="tag-selection"
                  labelKey="name"
                  multiple={true}
                  options={tags}
                  onChange={setSelected}
                  placeholder="Choose a tag..."
                  selected={selected}
                />
              </div>
            </Fragment>
          </Form.Group>

          <Button variant="secondary" onClick={handleClose}>
            Close
          </Button>
          <Button variant="primary" type="submit">
            Save Changes
          </Button>
        </Form>
      </Modal.Body>
      <Modal.Footer></Modal.Footer>
    </Modal>
  );
};
