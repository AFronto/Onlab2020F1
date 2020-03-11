import React, { FunctionComponent } from "react";

import {
  Card,
  CardBody,
  CardTitle,
  Button,
  Badge,
  DropdownItem
} from "reactstrap";
import Thread from "../../data/Threads/Thread";
import { useDispatch } from "react-redux";
import { removeThread } from "../../store/Threads";

export const ThreadCard: FunctionComponent<{ thread: Thread }> = props => {
  const dispatch = useDispatch();

  return (
    <Card>
      <CardBody>
        <CardTitle>{props.thread.name}</CardTitle>
        <DropdownItem divider />
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
      </CardBody>
    </Card>
  );
};
