import React, { FunctionComponent, useEffect } from "react";
import { Badge, Card, Col, Row, Spinner } from "react-bootstrap";
import { useDispatch, useSelector } from "react-redux";
import { useParams } from "react-router";
import { getSinglePost } from "../../../api/Post";
import { ReduxState } from "../../../store";
import { loadPost } from "../../../store/Thread/OpenPost";
import { AnswerCard } from "./AnswerCard";

export const SinglePostScreen: FunctionComponent = () => {
  var { threadId, postId } = useParams();

  const dispatch = useDispatch();

  useEffect(() => {
    dispatch(getSinglePost(threadId || "", postId || ""));
    return function cleanup() {
      dispatch(loadPost({ post: {} }));
    };
  }, []);

  var post = useSelector((state: ReduxState) => state.open_post);

  return (
    <div className="h-100" style={{ paddingTop: 100 }}>
      {post.title ? (
        <div>
          <Row style={{ paddingBottom: 30 }}>
            <Col xs={12}>
              <Card>
                <Card.Header as="h5" style={{ background: "#41b3a3" }}>
                  {post.title}
                </Card.Header>
                <Card.Body>
                  <div
                    style={{ marginBottom: 10 }}
                    className="d-flex justify-content-between"
                  >
                    <h6>
                      <b>{post.creationDate}</b>
                    </h6>
                    <div className="d-flex flex-column justify-content-between">
                      <h6>
                        Score: <b>{post.score || 0}</b>
                      </h6>
                      <h6>
                        Favorite Number: <b>{post.favoriteCount || 0}</b>
                      </h6>
                      <h6>
                        View Count: <b>{post.viewCount || 0}</b>
                      </h6>
                    </div>
                  </div>
                  <hr></hr>
                  <div
                    style={{ marginBottom: 10 }}
                    className="d-flex justify-content-between"
                  >
                    <div dangerouslySetInnerHTML={{ __html: post.body }} />
                  </div>
                  <hr></hr>
                  <div className="d-flex justify-content-between">
                    <h4>
                      {post.tags.map((tag) => (
                        <Badge style={{ margin: 5 }} pill variant="warning">
                          {tag}
                        </Badge>
                      ))}
                    </h4>
                  </div>
                </Card.Body>
              </Card>
            </Col>
          </Row>
          <Row>
            {post.answers.map((answer) => (
              <Col xs={{ span: 10, offset: 1 }} style={{ paddingBottom: 30 }}>
                <AnswerCard
                  answer={answer}
                  acceptedAnswer={post.acceptedAnswerId}
                />
              </Col>
            ))}
          </Row>
        </div>
      ) : (
        <div className="d-flex justify-content-center align-items-center h-100">
          <Spinner animation="border" />
        </div>
      )}
    </div>
  );
};
