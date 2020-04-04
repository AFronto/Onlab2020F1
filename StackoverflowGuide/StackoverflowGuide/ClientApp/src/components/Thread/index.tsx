import React, { FunctionComponent, useEffect } from "react";
import { ThreadCard } from "./ThreadCard";
import { Row, Col, Button } from "react-bootstrap";
import { useSelector, useDispatch } from "react-redux";
import { ReduxState } from "../../store";

import { createNewThread, getThreads } from "../../api/Thread";

export const ThreadsScreen: FunctionComponent = () => {
  const dispatch = useDispatch();

  useEffect(() => {
    dispatch(getThreads());
  }, []);

  // dispatch(
  //   addThread({ newThread: { id: "3", name: "IoT", tagList: ["C", "C++"] } })
  // );
  // dispatch(removeThread({ threadId: "0" }));
  const threads = useSelector((state: ReduxState) => state.threads);

  return (
    <div className="h-100">
      <Button
        style={{
          marginTop: 100,
          marginBottom: 100
        }}
        onClick={() => {
          // dispatch(
          //   addThread({
          //     newThread: { id: "3", name: "IoT", tagList: ["C", "C++"] }
          //   })
          // );
          dispatch(
            createNewThread({ id: "3", name: "IoT", tagList: ["C", "C++"] })
          );
        }}
      >
        Add Thread
      </Button>
      <Row style={{ marginBottom: 100 }}>
        {threads.map(threadCard => (
          <Col>
            <ThreadCard thread={threadCard} />
          </Col>
        ))}
      </Row>
    </div>
  );
};
