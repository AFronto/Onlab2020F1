import React, {
  FunctionComponent,
  useEffect,
  useState,
  useCallback,
} from "react";
import { ThreadCard } from "./ThreadCard";
import { FaPlus } from "react-icons/fa";
import { Row, Col, Button } from "react-bootstrap";
import { useSelector, useDispatch } from "react-redux";
import { ReduxState } from "../../store";

import { createNewThread, getThreads } from "../../api/Thread";
import { addThread } from "../../store/Thread";
import { ThreadModal } from "./ThreadModal";

export const ThreadsScreen: FunctionComponent = () => {
  const dispatch = useDispatch();

  const [show, setShow] = useState(false);

  const handleClose = useCallback(() => setShow(false), [setShow]);
  const handleShow = () => setShow(true);

  useEffect(() => {
    dispatch(getThreads());
  }, []);

  const threads = useSelector((state: ReduxState) => state.threads);

  return (
    <>
      <div className="h-100" style={{ paddingTop: 100 }}>
        <Row>
          {threads.map((threadCard) => (
            <Col xl={4} md={6}>
              <ThreadCard thread={threadCard} />
            </Col>
          ))}
        </Row>
        <div
          style={{
            position: "fixed",
            bottom: 0,
            right: 0,
            zIndex: 1,
            marginBottom: 30,
            marginRight: 30,
          }}
        >
          <Button
            size="lg"
            variant="success"
            onClick={() => {
              handleShow();
            }}
          >
            <FaPlus />
          </Button>
        </div>
      </div>

      <ThreadModal model={{ show, handleClose }} isNew={true} />
    </>
  );
};
