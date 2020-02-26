import React, { FunctionComponent } from "react";
import { ThreadCard } from "./ThreadCard";
import { Row, Col } from "reactstrap";

export const ThreadsScreen: FunctionComponent = () => {
  const threadList = [
    { id: "0", name: "Web Design", tagList: ["React", "HTML", "Angular"] },
    { id: "1", name: "Backend", tagList: ["C#", "C++"] },
    { id: "2", name: "Android", tagList: ["Kotlin"] }
  ];

  return (
    <div className="d-flex align-items-center" style={{ height: "100%" }}>
      <Row style={{ width: "100%" }}>
        {threadList.map(threadCard => (
          <Col>
            <ThreadCard thread={threadCard} />
          </Col>
        ))}
      </Row>
    </div>
  );
};
