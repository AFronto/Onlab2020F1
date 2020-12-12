import React, {
  FunctionComponent,
  useEffect,
  useCallback,
  useRef,
  useState,
} from "react";
import { useParams } from "react-router";
import { useDispatch, useSelector } from "react-redux";
import { getSingleThread } from "../../../api/Thread";
import { ReduxState } from "../../../store";
import { loadSingleThread } from "../../../store/Thread/SingleThread/OpenThread";
import { Graph } from "react-d3-graph";
import { graphData, customLabelBuilder } from "./logic/graphGeneration";
import { Row, Col, Spinner } from "react-bootstrap";
import { PostCard } from "./ThreadPost";
import ScrollArea from "react-scrollbar";
import { useWindowSize } from "../../../general_helpers/WindowHelper";
import { loadSuggestions } from "../../../store/Thread/SingleThread/Suggestions";
import PostData from "../../../data/server/Post/PostData";
import SingleThreadData from "../../../data/server/Thread/SingleThreadData";
import {
  getSuggestionsAfterDecline,
  getSuggestionsAfterAccept,
  deleteWatched,
} from "../../../api/Post";

export const SingleThreadScreen: FunctionComponent = () => {
  /////////////////////////////////////////////////////////////////////////////////////
  /////////////////////////////////// Initialization //////////////////////////////////
  var { id } = useParams();
  const dispatch = useDispatch();
  const [found, setFound] = useState<PostData | undefined>(undefined);

  useEffect(() => {
    if (id) {
      dispatch(getSingleThread(id));
    }

    return function cleanup() {
      dispatch(loadSingleThread({ singleThread: {} }));
      dispatch(loadSuggestions({ suggestions: [] }));
    };
  }, []);

  const open_thread = useSelector(
    (state: ReduxState) => state.single_thread.open_thread
  );

  const suggestions = useSelector(
    (state: ReduxState) => state.single_thread.suggestions
  );
  /////////////////////////////////////////////////////////////////////////////////////
  //////////////////////////////// Graph configuration ////////////////////////////////
  const size = useWindowSize();

  var postData = [] as PostData[];
  postData.push(
    ...suggestions.map((s) => {
      return { ...s, color: "#ffa31c" };
    })
  );
  if (open_thread.posts) {
    postData.push(...open_thread.posts);
  }

  const divRef = useRef<HTMLDivElement>(null);
  const scrollRef = useRef<ScrollArea>(null);

  const data = postData.length > 0 ? graphData(postData) : undefined;

  const myConfig = {
    nodeHighlightBehavior: false,
    automaticRearrangeAfterDropNode: true,

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
  /////////////////////////////////////////////////////////////////////////////////////
  ///////////////////////////////// Handler functions /////////////////////////////////
  const onClickNode = (nodeId: string) => {
    var clicked = postData.find((pD) => pD.id === nodeId)!;
    setFound(clicked);
    setTimeout(() => {
      setFound(undefined);
    }, 2000);
    if (clicked.threadIndex !== -1) {
      console.log((clicked.threadIndex - 1) * 140);
      scrollRef.current?.scrollYTo((clicked.threadIndex - 1) * 140);
    } else {
      scrollRef.current?.scrollBottom();
    }
  };

  const handleDeclineSuggestion = useCallback(
    (declined: PostData) => {
      const oldState = suggestions;

      dispatch(
        loadSuggestions({
          suggestions: suggestions.filter((s) => s.id !== declined.id),
        })
      );

      dispatch(
        getSuggestionsAfterDecline(open_thread.thread.id, oldState, declined)
      );
    },
    [suggestions, open_thread, dispatch]
  );

  const handleAcceptSuggestion = useCallback(
    (accepted: PostData) => {
      const oldState = {
        suggestions: suggestions,
        singleThread: open_thread,
      };

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
                  open_thread.posts.length > 0
                    ? open_thread.posts[open_thread.posts.length - 1]
                        .threadIndex + 1
                    : 0,
              },
            ]),
          } as SingleThreadData,
        })
      );

      dispatch(
        getSuggestionsAfterAccept(open_thread.thread.id, oldState, accepted)
      );
    },
    [suggestions, open_thread, dispatch]
  );

  const handleDeleteWatched = useCallback(
    (deleted: PostData) => {
      const oldState = {
        suggestions: suggestions,
        singleThread: open_thread,
      };

      if (suggestions.some((s) => s.connectedPosts.includes(deleted.id))) {
        dispatch(
          loadSuggestions({
            suggestions: suggestions.map((s) => {
              return { ...s, connectedPosts: deleted.connectedPosts };
            }),
          })
        );
      }
      let newList = open_thread.posts.filter((p) => p.id !== deleted.id);
      dispatch(
        loadSingleThread({
          singleThread: {
            thread: open_thread.thread,
            posts: newList.map((p) => {
              if (p.connectedPosts.includes(deleted.id)) {
                return {
                  ...p,
                  connectedPosts: deleted.connectedPosts,
                  threadIndex: deleted.threadIndex,
                };
              } else {
                if (p.threadIndex > deleted.threadIndex) {
                  return {
                    ...p,
                    threadIndex: p.threadIndex - 1,
                  };
                }
                return p;
              }
            }),
          } as SingleThreadData,
        })
      );

      dispatch(deleteWatched(open_thread.thread.id, oldState, deleted));
    },
    [suggestions, open_thread, dispatch]
  );
  /////////////////////////////////////////////////////////////////////////////////////
  ///////////////////////////////////// Render ////////////////////////////////////////
  return (
    <div className="h-100" style={{ paddingTop: 100 }}>
      {data && open_thread.posts ? (
        <Row>
          <Col xl={4} xs={12}>
            <ScrollArea
              ref={scrollRef}
              style={{
                height: size.height ? size.height - 100 : size.height,
              }}
              smoothScrolling={true}
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
                        handleDeleteWatched={handleDeleteWatched}
                        found={found !== undefined && found.id === post.id}
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
                        handleDeleteWatched={handleDeleteWatched}
                        found={found !== undefined && found.id === post.id}
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
              onClickNode={onClickNode}
            />
          </Col>
        </Row>
      ) : (
        <div className="d-flex justify-content-center align-items-center h-100">
          <Spinner animation="border" />
        </div>
      )}
    </div>
  );
};
