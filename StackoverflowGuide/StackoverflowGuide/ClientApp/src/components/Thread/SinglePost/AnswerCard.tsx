import React, { FunctionComponent, useState, useCallback } from "react";
import { FaTrash, FaPencilAlt } from "react-icons/fa";

import { Card, Button, Badge } from "react-bootstrap";
import { push } from "connected-react-router";
import AnswerData from "../../../data/server/Answer/AnswerData";

export const AnswerCard: FunctionComponent<{ answer: AnswerData }> = (
  props
) => {
  const { answer } = props;

  return (
    <>
      <Card>
        <Card.Header as="h5" style={{ background: "DarkGray" }}></Card.Header>
        <Card.Body>{answer.body}</Card.Body>
      </Card>
    </>
  );
};
