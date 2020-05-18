import PostData from "../../../data/server/Post/PostData";
import React, { FunctionComponent, useRef, useState, MouseEvent } from "react";
import { Card, Button, Popover, Overlay } from "react-bootstrap";
import { FaEllipsisV, FaExternalLinkAlt } from "react-icons/fa";

export const PostCard: FunctionComponent<{
  post: PostData;
  isSuggestion: boolean;
  divRef: React.RefObject<HTMLDivElement>;
  handleDeclineSuggestion: (arg0: PostData) => void;
  handleAcceptSuggestion: (arg0: PostData) => void;
}> = (props) => {
  const suggestionOptions = [
    { title: "Accept", action: () => {} },
    { title: "Decline", action: () => {} },
  ];

  const [show, setShow] = useState(false);
  const [target, setTarget] = useState<Element>();

  const handleClick = (event: MouseEvent) => {
    setShow(!show);
    setTarget(event.currentTarget);
  };

  const popover = (
    <Popover id="popover-basic">
      <Popover.Title as="h3">Options</Popover.Title>
      <Popover.Content>
        <div className="d-flex justify-content-between">
          {suggestionOptions.map((option) => (
            <Button
              variant={
                option.title === "Accept" ? "outline-success" : "outline-danger"
              }
              className={
                option.title === "Accept"
                  ? "border border-success"
                  : "border border-danger ml-2"
              }
              onClick={() => {
                setShow(false);
                if (option.title === "Accept") {
                  props.handleAcceptSuggestion(props.post);
                } else {
                  props.handleDeclineSuggestion(props.post);
                }
              }}
            >
              {option.title}
            </Button>
          ))}
        </div>
      </Popover.Content>
    </Popover>
  );

  return (
    <Card
      style={{ width: "18rem", marginBottom: 30 }}
      className="mx-auto"
      border={props.isSuggestion ? "warning" : "primary"}
    >
      <Card.Body>
        <div className="d-flex align-items-center justify-content-between">
          <Card.Title>{props.post.title}</Card.Title>
          {props.isSuggestion ? (
            <Button
              className="ml-4"
              style={{ color: "#444" }}
              variant="link"
              onClick={handleClick}
            >
              <h4>
                <FaEllipsisV />
              </h4>
            </Button>
          ) : (
            <Button className="ml-4" style={{ color: "#444" }} variant="link">
              <h4>
                <FaExternalLinkAlt />
              </h4>
            </Button>
          )}
          {null !== props.divRef.current && target && (
            <Overlay
              show={show}
              target={target}
              container={props.divRef.current}
              placement={"left"}
            >
              {popover}
            </Overlay>
          )}
        </div>
      </Card.Body>
    </Card>
  );
};
