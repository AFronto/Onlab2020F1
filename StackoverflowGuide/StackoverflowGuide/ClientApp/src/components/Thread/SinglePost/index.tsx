import React, { FunctionComponent, useEffect } from "react";
import { Badge, Card, Col, Row, Spinner } from "react-bootstrap";
import { useDispatch, useSelector } from "react-redux";
import { useParams } from "react-router";
import { getSinglePost } from "../../../api/Post";
import PostData from "../../../data/server/Post/PostData";
import { ReduxState } from "../../../store";
import { loadPost } from "../../../store/Thread/OpenPost";
import { AnswerCard } from "./AnswerCard";

export const SinglePostScreen: FunctionComponent = () => {
  var { threadId, postId } = useParams();
  console.log(threadId);
  console.log(postId);

  const dispatch = useDispatch();

  useEffect(() => {
    dispatch(getSinglePost(threadId || "", postId || ""));
  }, []);

  const post = useSelector((state: ReduxState) => state.open_post);

  //TEST
  // const post = {
  //   title: "Post title",
  //   body: "Post body",
  //   tags: ["alma", "korte", "repa"],
  //   creationDate: "2019-01-01",
  //   score: 10,
  //   favoriteNumber: 2,
  //   viewCount: 54,
  // };

  // const answers = [
  //   { id: "1", body: "Answer1 body", creationDate: "2020-01-01", score: 10 },
  //   { id: "2", body: "Answer2 body", creationDate: "2020-03-01", score: 4 },
  // ];

  return (
    <div className="h-100" style={{ paddingTop: 100 }}>
      {post.title ? (
        <div>
          <Row style={{ paddingTop: 100 }}>
            <Col xs={12}>
              <Card>
                <Card.Header as="h5" style={{ background: "DarkGray" }}>
                  {post.title}{" "}
                </Card.Header>
                <Card.Body>
                  <label> {post.creationDate} </label>
                  <div
                    style={{ marginBottom: 10 }}
                    className="d-flex justify-content-between"
                  >
                    Score: {post.score}
                    Favorite Number: {post.favoriteCount}
                    View Count: {post.viewCount}
                  </div>
                  <hr></hr>
                  <div
                    style={{ marginBottom: 10 }}
                    className="d-flex justify-content-between"
                  >
                    {post.body}
                  </div>
                  <hr></hr>
                  <div
                    style={{ marginBottom: 10 }}
                    className="d-flex justify-content-between"
                  >
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
              <Col xs={12} style={{ paddingTop: 50 }}>
                <AnswerCard answer={answer} />
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
