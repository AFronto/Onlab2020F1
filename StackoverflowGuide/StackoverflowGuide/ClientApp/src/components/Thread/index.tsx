import React, { FunctionComponent, useEffect } from "react";
import { ThreadCard } from "./ThreadCard";
import { Row, Col } from "react-bootstrap";
import { useSelector, useDispatch } from "react-redux";
import { ReduxState } from "../../store";
import { loadThreads, addThread, removeThread } from "../../store/Thread";

export const ThreadsScreen: FunctionComponent = () => {
  const threadList = [
    { id: "0", name: "Web Design", tagList: ["React", "HTML", "Angular"] },
    { id: "1", name: "Backend", tagList: ["C#", "C++"] },
    { id: "2", name: "Android", tagList: ["Kotlin"] }
  ];

  const dispatch = useDispatch();

  useEffect(() => {
    dispatch(loadThreads({ threadList: threadList }));
  }, []);

  // dispatch(
  //   addThread({ newThread: { id: "3", name: "IoT", tagList: ["C", "C++"] } })
  // );
  // dispatch(removeThread({ threadId: "0" }));
  const threads = useSelector((state: ReduxState) => state.threads);

  return (
    <Row>
      {threads.map(threadCard => (
        <Col>
          <ThreadCard thread={threadCard} />
        </Col>
      ))}
    </Row>
  );
};
