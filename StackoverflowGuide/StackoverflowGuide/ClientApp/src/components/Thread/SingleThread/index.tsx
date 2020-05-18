import React, {
  FunctionComponent,
  useEffect,
  useCallback,
  useRef,
} from "react";
import { useParams } from "react-router";
import { useDispatch, useSelector } from "react-redux";
import { getSingleThread } from "../../../api/Thread";
import { ReduxState } from "../../../store";
import { loadSingleThread } from "../../../store/Thread/SingleThread/OpenThread";
import { Graph } from "react-d3-graph";
import { graphData, customLabelBuilder } from "./logic/graphGeneration";
import { Row, Col } from "react-bootstrap";
import { PostCard } from "./ThreadPost";
import ScrollArea from "react-scrollbar";
import { useWindowSize } from "../../../general_helpers/WindowHelper";
import { loadSuggestions } from "../../../store/Thread/SingleThread/Suggestions";
import PostData from "../../../data/server/Post/PostData";
import SingleThreadData from "../../../data/server/Thread/SingleThreadData";

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

  const size = useWindowSize();

  var postData = [];
  postData.push(
    ...suggestions.map((s) => {
      return { ...s, color: "#ffa31c" };
    })
  );
  if (open_thread.posts) {
    postData.push(...open_thread.posts);
  }

  const data = postData.length > 0 ? graphData(postData) : undefined;

  const myConfig = {
    nodeHighlightBehavior: true,
    node: {
      color: "#38a1f4",
      size: 120,
      highlightStrokeColor: "#a840ba",
      labelProperty: customLabelBuilder,
    },
    link: {
      color: "#38a1f4",
      highlightColor: "#a840ba",
    },
    height: size.height ? size.height - 100 : size.height,
    width: 700,
  };

  const handleDeclineSuggestion = useCallback(
    (declined: PostData) => {
      dispatch(
        loadSuggestions({
          suggestions: suggestions.filter((s) => s.id !== declined.id),
        })
      );
    },
    [suggestions]
  );

  const handleAcceptSuggestion = useCallback(
    (accepted: PostData) => {
      dispatch(
        loadSuggestions({
          suggestions: suggestions
            .filter((s) => s.id !== accepted.id)
            .map((s) => {
              return { ...s, connectedPosts: [accepted.id] };
            }),
        })
      );
      dispatch(
        loadSingleThread({
          singleThread: {
            thread: open_thread.thread,
            posts: open_thread.posts.concat([
              {
                ...accepted,
                threadIndex:
                  open_thread.posts[open_thread.posts.length - 1].threadIndex +
                  1,
              },
            ]),
          } as SingleThreadData,
        })
      );
    },
    [suggestions]
  );

  const divRef = useRef<HTMLDivElement>(null);

  return (
    <div className="h-100" style={{ paddingTop: 100 }}>
      {data && open_thread.posts && (
        <Row>
          <Col xl={4} xs={12}>
            <ScrollArea
              style={{
                height: size.height ? size.height - 100 : size.height,
              }}
              speed={0.8}
              className="area"
              contentClassName="content"
              contentStyle={{
                minHeight: size.height ? size.height - 100 : size.height,
              }}
              horizontal={false}
            >
              <div ref={divRef}>
                {open_thread.posts.length > 0 && (
                  <div className="h3">Watched Posts</div>
                )}
                <Row>
                  {open_thread.posts.map((post) => (
                    <Col xl="auto" md={6}>
                      <PostCard
                        post={post}
                        isSuggestion={false}
                        divRef={divRef}
                        handleDeclineSuggestion={handleDeclineSuggestion}
                        handleAcceptSuggestion={handleAcceptSuggestion}
                      />
                    </Col>
                  ))}
                </Row>
                {suggestions.length > 0 && (
                  <div className="h3">Suggestions</div>
                )}
                <Row>
                  {suggestions.map((post) => (
                    <Col xl="auto" md={6}>
                      <PostCard
                        post={post}
                        isSuggestion={true}
                        divRef={divRef}
                        handleDeclineSuggestion={handleDeclineSuggestion}
                        handleAcceptSuggestion={handleAcceptSuggestion}
                      />
                    </Col>
                  ))}
                </Row>
              </div>
            </ScrollArea>
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
