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

export const ThreadCard: FunctionComponent<{ thread: Thread }> = props => {
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
        <Button>Delete</Button>
      </CardBody>
    </Card>
  );
};
