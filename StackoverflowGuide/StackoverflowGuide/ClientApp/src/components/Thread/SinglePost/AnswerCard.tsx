import React, { FunctionComponent } from "react";
import { FaCheckCircle } from "react-icons/fa";
import { Card } from "react-bootstrap";
import AnswerData from "../../../data/server/Answer/AnswerData";

export const AnswerCard: FunctionComponent<{
  answer: AnswerData;
  acceptedAnswer: string | undefined;
}> = (props) => {
  const { answer, acceptedAnswer } = props;

  return (
    <Card>
      <Card.Header as="h5" style={{ background: "DarkOrange" }}>
        <div className="d-flex justify-content-end">
          {acceptedAnswer && acceptedAnswer === answer.id && (
            <FaCheckCircle color={"white"} size={24} />
          )}
        </div>
      </Card.Header>
      <Card.Body>
        <div
          style={{ marginBottom: 10 }}
          className="d-flex justify-content-between"
        >
          <h6>
            <b>{answer.creationDate}</b>
          </h6>
          <div className="d-flex flex-column justify-content-between">
            <h6>
              Score: <b>{answer.score || 0}</b>
            </h6>
          </div>
        </div>
        <hr></hr>
        <div className="d-flex justify-content-between">
          <div
            className="w-100"
            dangerouslySetInnerHTML={{ __html: answer.body }}
            style={{ display: "flex", flexFlow: "column" }}
          />
        </div>
      </Card.Body>
    </Card>
  );
};
