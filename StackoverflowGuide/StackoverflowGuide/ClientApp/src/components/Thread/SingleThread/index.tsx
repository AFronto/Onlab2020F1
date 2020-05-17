import React, { FunctionComponent, useEffect } from "react";
import { useParams } from "react-router";
import { useDispatch, useSelector } from "react-redux";
import { getSingleThread } from "../../../api/Thread";
import { ReduxState } from "../../../store";
import { loadSingleThread } from "../../../store/Thread/SingleThread/OpenThread";
import { Graph } from "react-d3-graph";
import { graphData, customLabelBuilder } from "./logic/graphGeneration";
import { Row, Col } from "react-bootstrap";
import { PostdCard } from "./ThreadPost";

export const SingleThreadScreen: FunctionComponent = () => {
  var { id } = useParams();

  const dispatch = useDispatch();

  useEffect(() => {
    if (id) {
      dispatch(getSingleThread(id));
    }

    return function cleanup() {
      dispatch(loadSingleThread({ singleThread: {} }));
    };
  }, []);

  const open_thread = useSelector(
    (state: ReduxState) => state.single_thread.open_thread
  );

  const suggestions = useSelector(
    (state: ReduxState) => state.single_thread.suggestions
  );

  var postData = [];
  postData.push(...suggestions);
  if (open_thread.posts) {
    postData.push(...open_thread.posts);
  }

  const data = postData.length > 0 ? graphData(postData) : undefined;

  const myConfig = {
    nodeHighlightBehavior: true,
    node: {
      color: "lightgreen",
      size: 120,
      highlightStrokeColor: "blue",
      labelProperty: customLabelBuilder,
    },
    link: {
      highlightColor: "lightblue",
    },
    height: 800,
    width: 700,
  };

  return (
    <div className="h-100" style={{ paddingTop: 100 }}>
      {data && open_thread.posts && (
        <Row>
          <Col xl={4} xs={12}>
            <Row>
              {open_thread.posts.map((post) => (
                <Col xl="auto" md={6}>
                  <PostdCard post={post} />
                </Col>
              ))}
            </Row>
          </Col>
          <Col xs="auto" className="d-none d-xl-block">
            <Graph
              id="graph-id" // id is mandatory, if no id is defined rd3g will throw an error
              data={data}
              config={myConfig}
            />
          </Col>
        </Row>
      )}
    </div>
  );
};
