import PostData from "../../../data/server/Post/PostData";
import React, { FunctionComponent, useState, MouseEvent } from "react";
import { Card, Button, Popover, Overlay } from "react-bootstrap";
import { FaEllipsisV } from "react-icons/fa";
import { useDispatch, useSelector } from "react-redux";
import { ReduxState } from "../../../store";
import { getSinglePost } from "../../../api/Post";
import { push } from "connected-react-router";

export const PostCard: FunctionComponent<{
  post: PostData;
  isSuggestion: boolean;
  divRef: React.RefObject<HTMLDivElement>;
  handleDeclineSuggestion: (arg0: PostData) => void;
  handleAcceptSuggestion: (arg0: PostData) => void;
  handleDeleteWatched: (arg0: PostData) => void;
  found?: boolean;
}> = (props) => {
  const suggestionOptions = [
    {
      index: 0,
      title: "Accept",
      action: () => {
        props.handleAcceptSuggestion(props.post);
      },
    },
    {
      index: 1,
      title: "Decline",
      action: () => {
        props.handleDeclineSuggestion(props.post);
      },
    },
  ];

  const open_thread = useSelector(
    (state: ReduxState) => state.single_thread.open_thread
  );
  const dispatch = useDispatch();

  const watchedOptions = [
    {
      index: 0,
      title: "Open",
      action: () => {
        dispatch(
          push(`/threads/${open_thread.thread.id}/post/${props.post.id}`)
        );
      },
    },
    {
      index: 1,
      title: "Delete",
      action: () => {
        props.handleDeleteWatched(props.post);
      },
    },
  ];

  const [show, setShow] = useState(false);
  const [target, setTarget] = useState<Element>();

  const handleClick = (event: MouseEvent) => {
    setShow(!show);
    setTarget(event.currentTarget);
  };

  const popover = (
    <Popover id="popover-basic">
      <Popover.Title as="h3">
        {props.isSuggestion ? "Save Suggestion?" : "Options"}
      </Popover.Title>
      <Popover.Content>
        <div className="d-flex justify-content-between">
          {(props.isSuggestion ? suggestionOptions : watchedOptions).map(
            (option) => (
              <Button
                variant={
                  option.index === 0 ? "outline-success" : "outline-danger"
                }
                className={
                  option.index === 0
                    ? "border border-success"
                    : "border border-danger ml-2"
                }
                onClick={() => {
                  setShow(false);
                  option.action();
                }}
              >
                {option.title}
              </Button>
            )
          )}
        </div>
      </Popover.Content>
    </Popover>
  );

  return (
    <Card
      style={{ width: "18rem", marginBottom: 30 }}
      className={props.found ? "mx-auto text-light" : "mx-auto"}
      bg={props.found ? (props.isSuggestion ? "warning" : "primary") : "light"}
      border={props.isSuggestion ? "warning" : "primary"}
    >
      <Card.Body>
        <div className="d-flex align-items-center justify-content-between">
          <Card.Title>{props.post.title}</Card.Title>
          {props.isSuggestion ? (
            <Button
              className="ml-4"
              style={{ color: props.found ? "#fff" : "#444" }}
              variant="link"
              onClick={handleClick}
            >
              <h4>
                <FaEllipsisV />
              </h4>
            </Button>
          ) : (
            <Button
              className="ml-4"
              style={{
                color: props.found ? "#fff" : "#444",
              }}
              variant="link"
              onClick={handleClick}
            >
              <h4>
                <FaEllipsisV />
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
