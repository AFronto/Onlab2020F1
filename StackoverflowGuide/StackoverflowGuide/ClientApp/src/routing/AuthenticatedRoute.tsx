import React, { FunctionComponent } from "react";
import { Route, Redirect } from "react-router";
import { ReduxState } from "../store";
import { useSelector, useDispatch } from "react-redux";
import { loadAuthData } from "../store/Auth";
import AuthData from "../data/Auth/AuthData";

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

  const dispatch = useDispatch();

  if (
    localStorage.getItem("jwtToken") !== undefined &&
    localStorage.getItem("jwtToken") !== null
  ) {
    const jwt: AuthData = {
      token: localStorage.getItem("jwtToken")!,
      tokenExpirationTime: parseInt(
        localStorage.getItem("jwtTokenExpirationTime")!
      ),
      id: localStorage.getItem("jwtId")!
    };
    dispatch(loadAuthData({ jwt: jwt }));

    return <Route {...rest} render={({ location }) => children} />;
  }

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
