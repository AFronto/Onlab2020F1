import React, { FunctionComponent } from "react";
import { Route, Redirect } from "react-router";
import { ReduxState } from "../store";
import { useSelector } from "react-redux";

interface IProps {
  exact?: boolean;
  path: string;
  children: React.ReactNode;
}

export const AuthenticatedRoute: FunctionComponent<IProps> = ({
  children,
  ...rest
}) => {
  const jwtToken = useSelector((state: ReduxState) => state.jwt.token);

  return (
    <Route
      {...rest}
      render={({ location }) =>
        jwtToken !== undefined ? (
          children
        ) : (
          <Redirect
            to={{
              pathname: "/login",
              state: { from: location }
            }}
          />
        )
      }
    />
  );
};
