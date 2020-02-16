import React, { FunctionComponent, Fragment } from "react";
import { useSelector, useDispatch } from "react-redux";
import { increment } from "../store/Counter";
import { ReduxState } from "../store";
import { Button } from "reactstrap";

export const Counter: FunctionComponent = () => {
  const counter = useSelector((state: ReduxState) => state.counter);
  const dispatch = useDispatch();

  return (
    <Fragment>
      <h1>Counter</h1>

      <p>This is a simple example of a React component.</p>

      <p aria-live="polite">
        Current count: <strong>{counter}</strong>
      </p>

      <Button color="primary" onClick={() => dispatch(increment())}>
        Increment
      </Button>
    </Fragment>
  );
};
